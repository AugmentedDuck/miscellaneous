#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <time.h>

#ifdef _WIN32
    #include <windows.h>
#else
    #include <unistd.h>
    #include <pthread.h>
#endif

#define MAX_ROUNDS 50000
#define SIMULATION_AMOUNT 10000000

typedef struct {
    int worker_id;
    int sims_per_worker;
    long *histogram;
} worker_args_t;

static inline uint32_t xorshift32(uint32_t *state)
{
    uint32_t x = *state;

    x ^= x << 13;
    x ^= x >> 17;
    x ^= x << 5;

    *state = x;
    return x;
}


static inline int simulate_game(uint32_t *rngState) 
{
    int rounds = 0;
    int consecutiveMisses = 0;
    int shotMask = 0;

    while (consecutiveMisses < 6) 
    {
        int roll = (int) (((uint64_t) xorshift32(rngState) * 6ULL) >> 32);
        int dice = 1 << roll;

        int hit = ((shotMask & dice) ^ dice) >> roll;

        shotMask ^= dice;
        rounds += hit;

        consecutiveMisses = (consecutiveMisses + 1) & (hit - 1);
    }

    return rounds;
}

#ifdef _WIN32
DWORD WINAPI worker(LPVOID arg)
#else
void *worker(void *arg)
#endif
{
    worker_args_t *args = (worker_args_t *)arg;

    uint32_t seed = (uint32_t)(args->worker_id + 1) * 123456789u;

    for (int i = 0; i < args->sims_per_worker; i++)
    {
        int rounds = simulate_game(&seed);

        if (rounds < MAX_ROUNDS)
        {
            args->histogram[rounds]++;
        }
    }

    return 0;
}

long long current_time_ms()
{
    #ifdef _WIN32
        return GetTickCount64();
    #else
        struct timespec ts;
        clock_gettime(CLOCK_MONOTONIC, &ts);
        return (long long)(ts.tv_sec) * 1000 + ts.tv_nsec / 1000000;
    #endif
}

int main()
{
    printf("Simulating %d simulations\n", SIMULATION_AMOUNT);
    long long start = current_time_ms();

    int processor_count;

    #ifdef _WIN32
        SYSTEM_INFO sysinfo;
        GetSystemInfo(&sysinfo);
        processor_count = sysinfo.dwNumberOfProcessors;
    #else
        processor_count = sysconf(_SC_NPROCESSORS_ONLN);
    #endif
    
    int sims_per_worker = SIMULATION_AMOUNT / processor_count;

    long **worker_histograms = malloc(processor_count * sizeof(long *));
    for (int i = 0; i < processor_count; i++)
    {
        worker_histograms[i] = calloc(MAX_ROUNDS, sizeof(long));
    }
   
    #ifdef _WIN32
        HANDLE *threads = malloc(processor_count * sizeof(HANDLE));
    #else
        pthread_t *threads = malloc(processor_count * sizeof(pthread_t));
    #endif

    worker_args_t *args = malloc(processor_count * sizeof(worker_args_t));

    for (int i = 0; i < processor_count; i++)
    {
        args[i].worker_id = i;
        args[i].sims_per_worker = sims_per_worker;
        args[i].histogram = worker_histograms[i];
        
        #ifdef _WIN32
            threads[i] = CreateThread(
                            NULL,              // default security
                            0,                 // default stack size
                            worker,            // thread function
                            &args[i],          // argument
                            0,                 // run immediately
                            NULL               // thread ID (optional)
                        );
        #else
            pthread_create(&threads[i], NULL, worker, &args[i]);
        #endif
    }

    for (int i = 0; i < processor_count; i++)
    {
        #ifdef _WIN32
            WaitForSingleObject(threads[i], INFINITE);
        #else
            pthread_join(threads[i], NULL);
        #endif
    }

    for (int i = 0; i < processor_count; i++)
    {
        CloseHandle(threads[i]);
    }

    long *globalHistogram = calloc(MAX_ROUNDS, sizeof(long));

    for (int i = 0; i < MAX_ROUNDS; i++)
    {
        for (int w = 0; w < processor_count; w++)
        {
            globalHistogram[i] += worker_histograms[w][i];
        }
    }

    long long end = current_time_ms();

    printf("Simulated %d games in %lld ms\n", SIMULATION_AMOUNT, (end - start));

    // Cleanup
    for (int i = 0; i < processor_count; i++)
    {
        free(worker_histograms[i]);
    }
    free(worker_histograms);
    free(globalHistogram);
    free(threads);
    free(args);


    return 0;
}