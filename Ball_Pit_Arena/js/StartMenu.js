//Start menu and choices variables
var typeChoice;                             //this will be the select tag containing the choices of game types
var difficultyChoice;                       //this will be the select tag containing the choices of game difficulty levels
var optionDiv;                              //this will be the div that the select and button tags are in
var startButton;                            //this will be the button to choose the option in the choice select tag
var rules;                                  //this will be the paragraph tag in the optionDiv to explain the game rules and controls
var volumeOn;                               //this will be a boolean determining if the volume will be on or off
var volumeButton;						    //this will toggle the music volume being on or off
var ballAnimation;                          //this will be a checkbox to enable or disable the shoot animation
var shootAnimation                          //this will hold the value of the shoot animation (true == enabled, false == disabled)

function CreateOptionsDiv(){                                    //create the div for the game mode selection and add it to the body
    optionDiv = document.createElement("div");
    document.body.insertBefore(optionDiv, document.body.childNodes[1]);

    rules = document.createElement("p");
    rules.id = "rules";
    WriteRules();

    optionDiv.appendChild(rules);
}

function WriteRules(){
    Object.keys(gameExplanation).forEach(function(key) {        //loop through the keys for the object
        for(var i = 0; i < gameExplanation[key].length; i++){   //loop through the keys arrays to get their text
            rules.innerHTML += gameExplanation[key][i];
        }
    });
}

function CreateOptionsBox(){                                    //create select and choose button to select the game mode to play
    typeChoice = document.createElement("select");                  //create select tag to choose the game mode to play
    typeChoice.id = "typeChoice";
    optionDiv.appendChild(typeChoice);

    difficultyChoice = document.createElement("select");                  //create select tag to choose the game mode to play
    difficultyChoice.id = "difficultyChoice";
    optionDiv.appendChild(difficultyChoice);

    FillSelect(typeChoice, gameTypes);
    FillSelect(difficultyChoice, gameDifficultyLevels);    

    startButton = document.createElement("button");                  //create the button to start the game
    startButton.innerHTML = "Start Game";
    startButton.id = "start";
    optionDiv.appendChild(startButton);
    
    optionDiv.innerHTML += "<br>";

    volumeButton = document.createElement("button");                 //create the button to turn the music on or off
    volumeButton.innerHTML = "Turn Music Off";
    volumeButton.id = "volume";
    optionDiv.appendChild(volumeButton);

    ballAnimation = document.createElement('input');
    ballAnimation.type = "checkbox";
    ballAnimation.defaultChecked = true;
    ballAnimation.id = "ball";
    optionDiv.appendChild(ballAnimation);
    ballAnimation.outerHTML += "Enable Shoot Animation";

    //FUCK!   
}

function FillSelect(select, arr){
    for(var i = 0; i < arr.length; i++){                  //add option to the select list for each item in the array
        option = document.createElement("option");
        option.value = arr[i];
        option.text = arr[i];
        select.appendChild(option);
    }
}

function ToggleVolume(){
    if(volumeButton.innerHTML == "Turn Music Off"){
        volumeOn = false;
        volumeButton.innerHTML = "Turn Music On";
        if(deathSound != null){
            deathSound.mute();
        }
        if(backgroundMusic != null){
            backgroundMusic.mute();
        }
    }
    else{
        volumeOn = true;
        volumeButton.innerHTML = "Turn Music Off";
        if(deathSound != null){
            deathSound.unmute();
        }
        if(backgroundMusic != null){
            backgroundMusic.unmute();
        }
    }
}

function ToggleShootAnimation(){
    shootAnimation = $("#ball").prop("checked");
}