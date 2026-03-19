#define _CRT_SECURE_NO_WARNINGS 1

#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <omp.h>
#include <vector>

#define MAX_ROUNDS 40000

static inline uint32_t xorshift32(uint32_t* state) {
    uint32_t x = *state;
    x ^= x << 13;
    x ^= x >> 17;
    x ^= x << 5;
    return *state = x;
}

static inline int simulate(uint32_t* rng) {
    int rounds = 0;
    int current = 0;
    uint8_t mask = 0;

    while (current != 6) {
        int dice = xorshift32(rng) % 6;

        if (!(mask & (1 << dice))) {
            mask |= (1 << dice);
            rounds++;
            current = 0;
        }
        else {
            mask &= ~(1 << dice);
            current++;
        }
    }

    return rounds;
}

int main() {
    const int simulations = 1000000;

    std::vector<int> global_hist(MAX_ROUNDS, 0);

#pragma omp parallel
    {
        uint32_t rng = 123456789 ^ omp_get_thread_num();

        std::vector<int> local_hist(MAX_ROUNDS, 0);

#pragma omp for
        for (int i = 0; i < simulations; i++) {
            int r = simulate(&rng);
            if (r < MAX_ROUNDS)
                local_hist[r]++;
        }

        // merge
#pragma omp critical
        {
            for (int i = 0; i < MAX_ROUNDS; i++) {
                global_hist[i] += local_hist[i];
            }
        }
    }


    // output histogram
    FILE* f = fopen("histogram.csv", "w");
    fprintf(f, "Rounds;Count\n");

    int total = 0;
    for (int i = 0; i < MAX_ROUNDS; i++) {
        if (global_hist[i] > 0) {
            fprintf(f, "%d,%d\n", i, global_hist[i]);
            total += global_hist[i];
        }
    }

    fclose(f);

    printf("Done. Total: %d\n", total);
    return 0;
}