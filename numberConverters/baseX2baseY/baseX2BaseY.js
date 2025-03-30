function main(input, baseX, baseY) {

    let inputAsBase10 = TurnToBase10(input, baseX);

    let inputAsBaseY = TurnToBaseY(inputAsBase10, baseY);

    console.log(input + " : " + inputAsBase10  + " : " +inputAsBaseY);

    return inputAsBaseY;
}

function TurnToBase10(input, baseX) {
    let inputAsBase10 = 0;
    let inputAsString = input.toString();
    let inputLength = inputAsString.length;

    for (let i = 0; i < inputLength; i++) {
        let digit = parseInt(inputAsString.charAt(i), baseX);
        inputAsBase10 += digit * Math.pow(baseX, inputLength - i - 1);
    }

    return inputAsBase10;
}

function TurnToBaseY(input, baseY) {
    let inputAsBaseY = '';
    let inputAsBaseYArray = [];

    while (input > 0) {
        let remainder = input % baseY;
        inputAsBaseYArray.unshift(remainder);
        input = Math.floor(input / baseY);
    }

    for (let i = 0; i < inputAsBaseYArray.length; i++) {
        inputAsBaseY += inputAsBaseYArray[i].toString(baseY);
    }

    return inputAsBaseY;
}