let actionButton = document.getElementById("actionButton");
let inputText = document.getElementById("inputText");
let outputText = document.getElementById("outputText");
let key1 = document.getElementById("key1");
let key2 = document.getElementById("key2");

actionButton.onclick = function() {
    let text = inputText.value;

    let key1Value = key1.value;
    let key2Value = key2.value;

    if (key1Value.length != key2Value.length) {
        alert("Keys must be of the same length!");
        return;
    }

    let output = "";

    for (const char in text) {
        if (key1Value.indexOf(text[char]) == -1) {
            output += text[char];
            continue;
        }

        output += key2Value[key1Value.indexOf(text[char])];
    }

    outputText.value = output;
}