
function generateDetails() {
    let firstName = document.getElementById("firstName").value;
    let lastName = document.getElementById("lastName").value;

    let email = document.getElementById("email").value;
    let phone = document.getElementById("phone").value;

    let twitter = document.getElementById("xHandle").value;
    let github = document.getElementById("github").value;

    let name = lastName + ", " + firstName;

    if (!verifyEmail(email)) {
        alert("Please enter a valid email address.");
        return;
    }

    if (!verifyTwitter(twitter)) {
        alert("Please enter a valid x handle.");
        return;
    }
}

function verifyEmail(email) {
    // Regular expression for validating email address
    const emailRegex = /([A-Za-z0-9\.\+]+)@[A-Za-z0-9]+\.[A-Za-z0-9]+/;
    return emailRegex.test(email);
}

function verifyTwitter(twitter) {
    return twitter.startsWith("@") && twitter.length > 1;
}