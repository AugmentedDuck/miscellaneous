
window.onload = setup

function setup() {
    let content = document.getElementById('container')

    let standardScreen = ``

    for (let i = 1; i < 13; i++) {
        standardScreen += `<div class="district">
                    <h2>District ${i}</h2>`;
        for (let j = 1; j < 3; j++) {
            standardScreen += `<div class="player">
                        <p>ID: ${i}-${j}</p>
                        <p>Name:</p>
                        <input type="text" name="player${i}-${j}Name" id="player${i}-${j}Name">
                        <p>Sex:</p>
                        <input type="text" name="player${i}-${j}Sex" id="player${i}-${j}Sex">
                        <p>Age:</p>
                        <input type="text" name="player${i}-${j}Age" id="player${i}-${j}Age">
                        <p>Alliances:</p>
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance1-2"> 1-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance1-2"> 1-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance2-1"> 2-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance2-2"> 2-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance3-1"> 3-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance3-2"> 3-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance4-1"> 4-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance4-2"> 4-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance5-1"> 5-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance5-2"> 5-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance6-1"> 6-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance6-2"> 6-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance7-1"> 7-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance7-2"> 7-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance8-1"> 8-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance8-2"> 8-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance9-1"> 9-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance9-2"> 9-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance10-1"> 10-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance10-2"> 10-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance11-1"> 11-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance11-2"> 11-2
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance12-1"> 12-1
                        <input type="checkbox" name="player${i}-${j}Alliance" id="player${i}-${j}Alliance12-2"> 12-2
                    </div>`;
        }
        standardScreen += `</div>`;
    }
    
    content.innerHTML = standardScreen
}