function ToPigLatin(input) {
    const consonants = /^[^aeiou]/i; // Matches any consonant at the start of the string

    let inputArray = input.split(" ");

    let pigLatinArray = inputArray.map(word => {
        if (consonants.test(word)) {
            // If the word starts with a consonant, move it to the end and add "ay"
            return word.slice(1) + word[0] + "ay";
        } else {
            // If it starts with a vowel, just add "yay" at the end
            return word + "yay";
        }
    });

    let pigLatinString = pigLatinArray.join(" ");

    console.log(pigLatinString);

    return pigLatinString;
}