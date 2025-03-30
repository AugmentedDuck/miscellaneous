function roman2Decimal(input) {
  const romanNumerals = {
        I: 1,
        V: 5,
        X: 10,
        L: 50,
        C: 100,
        D: 500,
        M: 1000
    };

    let decimal = 0;
    let prevValue = 0;

    for (let i = input.length - 1; i >= 0; i--) {
        const currentValue = romanNumerals[input[i]];

        if (currentValue < prevValue) {
            decimal -= currentValue;
        } else {
            decimal += currentValue;
        }

        prevValue = currentValue;
    }

    console.log(`Roman numeral: ${input}, Decimal value: ${decimal}`);

    return decimal;
}

function decimal2Roman(input) {
    const romanNumerals = {
        1: 'I',
        4: 'IV',
        5: 'V',
        9: 'IX',
        10: 'X',
        40: 'XL',
        50: 'L',
        90: 'XC',
        100: 'C',
        400: 'CD',
        500: 'D',
        900: 'CM',
        1000: 'M'
    }

    let decimal = input;
    let roman = '';

    const keys = Object.keys(romanNumerals).reverse();
    
    for (let i = 0; i < keys.length; i++) {
        const key = parseInt(keys[i]);
        while (decimal >= key) {
            roman += romanNumerals[key];
            decimal -= key;
        }
    }

    console.log(`Roman numeral: ${roman}, Decimal value: ${input}`);

}