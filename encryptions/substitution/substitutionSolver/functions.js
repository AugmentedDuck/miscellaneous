import { words as dictionary } from './wordList.js';

const tranlationCache = new Map();
const dictionaryByLenght = new Map();
const dictionarySet = new Set(dictionary);
const outputFrequency = 1;
let outputCounter = 0;

for (const word of dictionary) {
    let length = word.length;
    if (!dictionaryByLenght.has(length)) {
        dictionaryByLenght.set(length, []);
    }
    dictionaryByLenght.get(length).push(word);
}

export function isWordGood(word) {
    if (dictionarySet.has(word)) {
        return true;
    }

    if (word == '.'.repeat(word.length)) {
        return true;
    }

    let tempDictionary = dictionaryByLenght.get(word.length) ?? [];
    if (tempDictionary.length <= 0) {
        return false;
    }

    outer:
    for (let candidate of tempDictionary) {
        for (let i = 0; i < word.length; i++) {
            if (word[i] != '.' && word[i] != candidate[i]) {
                continue outer;
            }
        }
        return true;
    }

    return false;

    /*
    for (let i = 0; i < word.length; i++) {
        let letter = word[i];
        if (letter != '.') {
            tempDictionary = tempDictionary.filter(w => w[i] == letter);
        }
    }

    return tempDictionary.length > 0;
    */
}

export function translateWord(word, key, alphabet = "abcdefghijklmnopqrstuvwxyz") {

    let cacheKey = word + ':' + key;
    let translatedWord = tranlationCache.get(cacheKey);
    
    if (translatedWord) {
        return translatedWord;
    }

    translatedWord = '';

    for (let i = 0; i < word.length; i++) {
        let letter = word[i];
        if (!alphabet.includes(letter)) {
            console.log("Letter not in alphabet: " + letter);
            translatedWord += letter;
        } else {
            let index = alphabet.indexOf(letter);
            translatedWord += key[index];
        }
    }

    return translatedWord;
}

export function solveKey(encodedWords, startingKey, alphabet, maxError, index = 0) {
    
    outputCounter++;
    
    if (index >= startingKey.length) {
        console.log("FOUND KEY: " + startingKey);
        return startingKey;
    }
    
    if (outputCounter % outputFrequency == 0) {
        console.log("Solving key for index: " + index + " with starting key: " + startingKey);
    }
    
    let goodKeys = [];
    let errorRates = [];

    for (let i = 0; i < alphabet.length; i++) {
        let letter = alphabet[i];

        if (startingKey.includes(letter)) {
            continue;
        }

        let currentKey = [...startingKey];
        currentKey[index] = alphabet[i];

        let errors = 0;

        for (const word of encodedWords) {
            let translatedWord = translateWord(word, currentKey.join(''), alphabet);
            if (!isWordGood(translatedWord)) {
                errors++;
                if (errors > maxError) {
                    break;
                }
            }
        }

        if (maxError >= errors) {
            goodKeys.push([...currentKey]);
            errorRates.push(errors);
        }
    }

    if (goodKeys.length <= 0) {
        console.log("No good keys found for index: " + index + " with starting key: " + startingKey);
        return startingKey;
    }

    goodKeys = goodKeys
        .map((key, index) => ({ key, errorRate: errorRates[index] }))
        .sort((a, b) => a.errorRate - b.errorRate)
        .map(item => item.key);

    errorRates = errorRates
        .sort((a, b) => a - b);
    
    let keyList = [];
    
    /*
    // SLOW BUT FINDS ALL KEYS
    for (const key of goodKeys) {
        keyList.push(solveKey(encodedWords, key, alphabet, maxError, index + 1));
    }

    keyList = keyList.filter(k => k !== undefined);

    return keyList;
    */

    //*
    // FAST BUT ONLY FINDS THE BEST KEYS SO FAR
    let i = 0;

    while (i < goodKeys.length && errorRates[i] <= errorRates[0]) {
        let key = goodKeys[i];

        keyList.push(solveKey(encodedWords, key, alphabet, maxError, index + 1));

        i++;
    }

    keyList = keyList.filter(k => k !== undefined);

    return keyList;

    // SUPER FAST BUT ONLY FINDS THE FIRST KEY
    //return solveKey(encodedWords, goodKeys[0], alphabet, maxError, index + 1);    
}

export function getLetterFrequency(text) {
    let letterFrequency = {};

    for (let i = 0; i < text.length; i++) {
        let letter = text[i].toLowerCase();
        if (letterFrequency[letter]) {
            letterFrequency[letter]++;
        } else {
            letterFrequency[letter] = 1;
        }
    }

    return letterFrequency;
}