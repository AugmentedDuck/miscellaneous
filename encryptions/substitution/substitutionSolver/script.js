import { solveKey, translateWord, getLetterFrequency } from './functions.js';

let actionButton = document.getElementById("actionButton");
let inputText = document.getElementById("inputText");
let outputText = document.getElementById("outputText");
let hint = document.getElementById("hint");

const MAX_WORKERS = navigator.hardwareConcurrency || 6; // Get the number of available CPU cores

actionButton.onclick = function() {
    const alphabet = "etaoinshrdlcumwfgypbvkjxqz";
    const errorRate = 0.9; // 10% of the words can be wrong (To compensate for made up words, typos, and names)
    const frequencyDetermined = Math.min( 10 / alphabet.length, 1);

    let text = inputText.value.toLowerCase();    
    let encodedWords = text.replace(/[^a-z ]/gi, '').split(' ');
    let maxError = errorRate * encodedWords.length;

    let textAlphabet = text.replace(/[^a-z]/gi, '').split('');
    let letterFrequency = getLetterFrequency(textAlphabet);
    let sortedFrequency = Object.keys(letterFrequency).sort((a, b) => letterFrequency[b] - letterFrequency[a]);

    let startingKey = '.'.repeat(alphabet.length).split('');
    
    for (let i = 0; i < Math.round(frequencyDetermined * alphabet.length); i++) {
        const letter = sortedFrequency[i];
        startingKey[i] = letter;
    }

    let taskQueue = [];
    let results = []
    let finished = 0;
    
    console.log("SOLVING...")

    let solvedKeys = solveKey(encodedWords, startingKey, alphabet, maxError, Math.round(frequencyDetermined * alphabet.length));

    let useKey = solvedKeys;

    console.log("KEY FOUND: " + useKey);

    if (solvedKeys && solvedKeys.length > 0) {
        outputText.value = translateWord(text, useKey.join(''), alphabet);
    }


    /*
    for (let i = 0; i < alphabet.length; i++) {
        const letter = alphabet[i];
        if (startingKey.includes(letter)) {
            continue;
        }

        const currentKey = [...startingKey];
        currentKey[0] = letter;

        taskQueue.push({ encodedWords, startingKey: currentKey, alphabet, maxError, index: 1 });
    }

    function startWorker(task) {
        const worker = new Worker('worker.js', { type: 'module' });

        worker.postMessage(task);

        worker.onmessage = (e) => {
            const keys = e.data;
            if (keys && keys.length > 0) {
                results.push(...keys);
            }

            finished++;

            if (finished === taskQueue.length) {
                console.log("Finished solving all keys")
                if (results.length > 0) {
                    outputText.value = translateWord(text, results[0], alphabet);
                } else {
                    outputText.value = "No keys found";
                }
            } else {
                if (taskQueue.length > 0) {
                    let nextTask = taskQueue.shift();
                    startWorker(nextTask);
                }
            }

            worker.terminate();
        };
    }

    for (let i = 0; i < Math.min(taskQueue.length, MAX_WORKERS); i++) {
        let task = taskQueue.shift();
        startWorker(task);
    }
    */
}