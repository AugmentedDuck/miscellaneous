import { solveKey } from './functions.js';

onmessage = function (e) {
    const { encodedWords, startingKey, alphabet, maxError, index} = e.data;

    if (!encodedWords || !startingKey || !alphabet ) {
        console.warn("Missing data in worker: ", e.data);
        return;
    }

    let solvedKeys = solveKey(encodedWords, startingKey, alphabet, maxError, index);

    postMessage(solvedKeys);
}