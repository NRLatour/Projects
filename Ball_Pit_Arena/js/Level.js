//level variables
var gameType;                                                       //this will contain the current game type
var gameTypes = ["Story", "Survival", "Horde", "Dodgeball"];         //this contains the names of each game type
var dodgeballRoundGoal;                                             //this will contain the round goal for the dodgeball game
var backgroundMusicSrc;                                             //this will contain the source of the background music
var gameDifficulty;                                                 //this will contain the current difficulty level
var gameDifficultyLevels = ["Medium", "Hard"];                      //this contains the different difficult levels available
var ballSpeedMult;                                                  //this will be the multipier to the ball speed
var spawnMult;                                                      //this will be the multipier to the ball speed
var pointsMult;                                                     //this will be the multipier for all points gained

var gameExplanation = {
    "intro": [
        "<b>Introduction</b><br>",
        "Welcome to Paintball Deathmatch, a different kind of strategy game.<br><br>",
    ],
    "rules": [
        "<b>RULES<b><br>",
        "In this game you will be a small green square placed near the center of the game board.<br>",
        "The enemies will be tomato red squares that are larger than you.<br>",
        "The game is played in a turn based system, you will be given one ball per turn that you can shoot at any point.<br>",
        "You will be able to shoot as many balls per turn as you have available. If you have not shot any, you will accumulate them.<br>",
        "You will always move one square at a time. <b>However, when you move, all balls and enemies will also move.</b><br>",
        "The goal of the game is to defeat the number of enemies that will be displayed at below the game board and get as many points as possible.<br>",
        "If you choose the Survival or Story game type, the enemies will also be able to shoot at you.<br>",
        "When you win, you will be given bonus points depending on the game type.<br><br>",
        "The different game types are: <br>",
        "<b>Story</b>: enemies will slowly appear and shoot at you, this is the basic game type.<br>",
        "<b>Survival</b>: you will need to dodge bullets and defeat every enemy to win, this is probably the hardest version.<br>",
        "<b>Horde</b>: you will be overwhelmed and careful planning will get you through.<br>",        
        "<b>Dodgeballl</b>: there are no enemies and you only need to dodge the balls until they all break apart.<br><br>",
        "You will also receive bonus points for the number of balls you accumulated and you will get more points the faster you win the level.<br>",
        "The ball size, the game board dimensions, and the number of enemies you need to beat will change with each game type.<br>",
        "The difficulty level will change the ball speed, give more points in the harder difficulties and change the number of spawned enemies<br><br>",
        "<b>WARNING: THIS GAME INCLUDES AUDIO THAT BEGINS WHEN YOU CLICK THE \"Start Game\" BUTTON<br><br>"
    ],
    "controls": [
        "<b>Controls<b><br>",
        "To move your character you have the choice of using the arrow keys or the WASD keys.<br>",
        "To shoot, you use the mouse to click in the direction that you wish to shoot.<br>"
    ]
}

var DifficultyMultipliers = {
    "Medium": {
        "ballSpeedMult": 2,
        "spawnMult": 1,
        "pointsMult": 1
    },
    "Hard": {
        "ballSpeedMult": 2.5,
        "spawnMult": 1.5,
        "pointsMult": 1.5        
    }
};

var LevelValues = {                                                 //this is the offline version of the json object containing the game mode statistics
    "Story": {
        "boardDimensions": 12, 
        "fps": 76, 
        "ballRadius": 4, 
        "shotsPerTurn": 1,
        "killPoints": 20,
        "winBonus": 100,
        "spawnRate": 2,
        "shootingEnemies": true,
        "totalNumberOfEnemies": 20,
        "numberOfWaves": 5,
        "goalText": "Defeat all enemies",
        "roundBonus": 0,
        "backgroundMusic": "./audio/Megalovania.mp3",
        "progressMessage": "ENEMIES LEFT: "
    },
    "Survival": {
        "boardDimensions": 16, 
        "fps": 76, 
        "ballRadius": 3, 
        "shotsPerTurn": 1,
        "killPoints": 20,
        "winBonus": 300,
        "spawnRate": 8,
        "shootingEnemies": true,
        "totalNumberOfEnemies": 500,
        "numberOfWaves": 1,
        "goalText": "Survive as long as possible",
        "roundBonus": 10,
        "backgroundMusic": "./audio/Megalovania.mp3",
        "progressMessage": "ENEMIES LEFT: "
    },
    "Horde": {
        "boardDimensions": 16, 
        "fps": 76, 
        "ballRadius": 3, 
        "shotsPerTurn": 1,
        "killPoints": 10,
        "winBonus": 200,
        "spawnRate": 8,
        "shootingEnemies": false,
        "totalNumberOfEnemies": 80,
        "numberOfWaves": 5,
        "goalText": "Defeat all enemies",
        "roundBonus": 0,
        "backgroundMusic": "./audio/Megalovania.mp3",
        "progressMessage": "ENEMIES LEFT: "
    },
    "Dodgeball": {
        "boardDimensions": 12, 
        "fps": 76, 
        "ballRadius": 5, 
        "shotsPerTurn": 0,
        "killPoints": 0,
        "winBonus": 500,
        "spawnRate": 8,
        "shootingEnemies": true,
        "totalNumberOfEnemies": 8,
        "numberOfWaves": 5,
        "goalText": "Survive until round 25",
        "roundBonus": 40,
        "backgroundMusic": "./audio/DangerZone.mp3",
        "progressMessage": "ROUNDS LEFT: "
    }    
}; 

var highScores = {
    "Survival": 0,
    "Horde": 0,
    "Story": 0,
    "Dodgeball": 0
}

function GetLevelStatistics(){ //get the level dependent values for the level and assign the values dependent on the newly defined variables
    //get LevelValues json
    //getLevelValues();                              //get json object from file when the game is hosted

    //get the different multipliers for different difficulty levels
    ParseDifficultyLevel();

    //level dependent values
    ParseLevelStats();

    //initialize the movement limits for the character
    AssignLevelLimits();

    //assign variable dependent values
    AssignVariableDependentValues();

    //assign the values that are not dependent on anything
    GetStaticGameStatistics();

    //empties all arrays
    ResetArrays();
}

function getLevelValues(){                                          //get json object from file when the game is hosted
    $.getJSON( "./json/LevelValues.json", function( data ) {
        LevelValues = data;        
    });
}

function AssignVariableDependentValues(){
    playerStartRow = boardDimensions / 2 - Math.floor(Math.random() * 2);       //randomize the starting row of the player
    playerStartCol = boardDimensions / 2 - Math.floor(Math.random() * 2);       //randomize the starting col of the player
    padding = 40/boardDimensions;
    tileSize = 400/boardDimensions;
    characterSize = tileSize * 3 / 5;
    playerSize = tileSize * 2 / 5;
    gameCanvasSize = Math.ceil(padding + boardDimensions*(tileSize + padding));
    statsCanvasHeight = gameCanvasSize / 2;      
    statsCanvasWidth = gameCanvasSize;      
    nextTile = padding + tileSize; 
    speed = nextTile / fps * 2;
    ballSpeed = speed * ballSpeedMult;
    moveTime = fps / 2;
}

function GetStaticGameStatistics(){                                 //This initializes statistics that are not level dependent
    currentScore = 0;
    currentRound = 1;
    ballsLeft = 0;
    killCount = 0;
    drawPlayer = true;                                                  //this variable will determine if the player still needs to be drawn
    enemiesSpawned = 0;                                                 //this will keep track of the number of enemies that have spawned
    roundSpeedBonus = roundBonus * 2;                                   //this will be the bonus points gained from beating a level wuickly
    ballsAccumulatedBonus = 10;                                         //this will be the bonus points gained from the number of balls accumulated at the end
    won = false;                                                        //this will determine whether the player has won or not yet
    newHighscoreAttained = false;                                       //this will determine if the player has reached a new highscore in the past round
    winResult = "YOU WON!";                                             //this is the message that is displayed when you win
    loseResult = "YOU DIED!";                                           //this is the message that is displayed when you lose
    yDisplayIncrement = 30;                                             //this will be the increment for distancing the results on the results screen
    displayX = 40;                                                      //this will be the default x position of the results displays
    volumeOn = (volumeOn == undefined)? true : volumeOn;                //this will give the current value of the volume button if it is not assigned
    ToggleShootAnimation();                                             //this will assign the current checked value of the Enable Shoot Animation checkbox to the shoot animation variable
    shootAnimationTickLimit = 6;                                        //this will be the animation limit for the shoot animation
    shootAnimationFrameSpeed = 80;                                      //this will be the millisecond interval speed of the shoot animation
    shootAnimationSafeDistDenom = 0;                                    //this will be the fraction of each movement of the balls during the shoot animation
    for(var i = 0; i < shootAnimationTickLimit; i++)    
        shootAnimationSafeDistDenom += shootAnimationTickLimit - i;     //this is a sum of the tickLimit to 1 to get the denominator
    activeAnimations = 0;                                               //this will hold the number of active animations

    //fireworks spritesheet variables
    fFramesPerRow = 1;
    fFramesPerCol = 8;
    fWidth = blueFireworks.width;
    fHeight = blueFireworks.height;
    fFrameWidth = fWidth / fFramesPerCol;
    fFrameHeight = fHeight / fFramesPerRow;
    fireworkFrameY = 0;

    //death spritesheet variables
    dFramesPerRow = 6;
    dFramesPerCol = 8;
    dWidth = deathPic.width;
    dHeight = deathPic.height;
    dFrameWidth = dWidth / dFramesPerCol;
    dFrameHeight = dHeight / dFramesPerRow;    
}

function ResetArrays(){                                              //this empties all of the arrays
    enemyEndTiles = [];
    pieces = [];
    board = [];
    balls = [];
    enemyBalls = [];
    resultsToDraw = [];
    resultsToAdd = [];
    resultsSwitch = [];
    displays = [];
    ScoreDistribution = {"Kills": 0, "SpeedBonus": 0,               //this will be used for displaying the results
"Rounds":0, "BallsRemaining": 0, "Total": 0};
}

function ParseLevelStats(){                                      //get gameType specific object call
    switch (gameType){
        case "Story": {              //lots of balls, lots of enemies
            AssignLevelStats(LevelValues.Story);
            break;
        }
        case "Survival": {          //lots of balls, lots of enemies, no end in sight
            AssignLevelStats(LevelValues.Survival);
            break;
        }
        case "Horde": {             //1 ball per turn, lots of enemies, get to the fifth wave
            AssignLevelStats(LevelValues.Horde);
            break;
        }
        case "Dodgeball": {         //no enemies, lots of balls per turn
            AssignLevelStats(LevelValues.Dodgeball);
            break;
        }
    }
}

function ParseDifficultyLevel(){                                //get difficulty specific object call
    switch (gameDifficulty){
        case "Medium": {          //regular game difficulty
            GetDifficultyStats(DifficultyMultipliers.Medium);
            break;
        }
        case "Hard": {             //harder game difficulty
            GetDifficultyStats(DifficultyMultipliers.Hard);
            break;
        }
    }
}

function GetDifficultyStats(diff){                              //assign the difficulty multipliers
    ballSpeedMult = diff.ballSpeedMult;                                                  
    spawnMult = diff.spawnMult;                                                      
    pointsMult = diff.pointsMult;                                                  
}

function AssignLevelStats(lvl){                                     //this gets and assigns the values of the variables specific to the game mode
    boardDimensions = lvl.boardDimensions;
    fps = lvl.fps;
    ballRadius = lvl.ballRadius;
    shotsPerTurn = lvl.shotsPerTurn;
    killPoints = lvl.killPoints;
    winBonus = lvl.winBonus;
    spawnRate = lvl.spawnRate * spawnMult;
    shootingEnemies = lvl.shootingEnemies;
    totalNumberOfEnemies = lvl.totalNumberOfEnemies * spawnMult;
    goalText = lvl.goalText;
    roundBonus = lvl.roundBonus;
    backgroundMusicSrc = lvl.backgroundMusic;
    progressMessage = lvl.progressMessage;
}

function AssignLevelLimits(){
    if(gameType == "Dodgeball"){
        rightLimit = boardDimensions - 1;             
        leftLimit = 1;
        topLimit = 1;              
        bottomLimit = boardDimensions - 1;
        dodgeballRoundGoal = 25;
        ToggleClickEvent();
        resultPositions = {"endResult": {"x":100, "y":60}, "finalScore": {"x":130,"y":100}};         //this will be used for displaying the results
    }
    else{
        rightLimit = boardDimensions;             
        leftLimit = 0;
        topLimit = 0;              
        bottomLimit = boardDimensions;
        resultPositions = {"endResult": {"x":100, "y":60}, "finalScore": {"x":130,"y":100}};         //this will be used for displaying the results
    }
}

function WinConditions(){
    switch (gameType){
        case "Story": {          //lots of balls, lots of enemies
            return(killCount == totalNumberOfEnemies); 
        }
        case "Survival": {          //lots of balls, lots of enemies, no end in sight
            return(killCount == totalNumberOfEnemies); 
        }
        case "Horde": {         //1 ball per turn, lots of enemies, get to the fifth wave
            return(killCount == totalNumberOfEnemies); 
        }
        case "Dodgeball": {     //no enemies, lots of balls per turn
            return(balls.length == 0 || currentRound >= dodgeballRoundGoal);
        }
    }
}

function SpawnConditions(){
    if(gameType == "Dodgeball") 
        return true;

    return(currentRound % 2 == 1 && spawnRate > 0 && totalNumberOfEnemies > enemiesSpawned);
}

