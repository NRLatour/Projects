//stats board variables
var statctx;                                                        //this will hold the value of the context of the stats canvas
var kills;                                                          //this will hold the text in the stats canvas keeping track of the killCount
var round;                                                          //this will hold the text in the stats canvas keeping track of the round number
var score;                                                          //this will hold the text in the stats canvas keeping track of the score
var progress;                                                       //this will hold the text in the stats canvas keeping track of the progress to the goal of the game type
var progressMessage                                                 //this will hold the message defining the progress made in the game type
var goal;                                                           //this will hold the text in the stats canvas saying the goal of the game
var ballCount;                                                      //this will hold the text in the stats canvas keeping track of the number of balls left for the player
var highscore;                                                      //this will hold the text in the stats canvas keeping track of the highscore for the game type
var highscore;                                                      //this will hold the text in the stats canvas keeping track of the current game type
var difficulty;                                                     //this will hold the text in the stats canvas keeping track of the current difficulty level
var currentScore;                                                   //this will hold the player's current score
var currentRound;                                                   //this will hold the current round
var killPoints;                                                     //this will be the number of points per kill
var winBonus;                                                       //the number of bonus points gained from winning the level
var killCount;                                                      //the number of enemies killed
var totalNumberOfEnemies;                                           //total number of enemies to win the round
var roundSpeedBonus;                                                //this will be the number of bonus points gained for winning the game under twice the number of turns per enemy
var ballsAccumulatedBonus;                                          //this will be the number of bonus points gained from saving up bullets
var roundBonus;                                                     //this will give the player points for surviving each round
var goalText;                                                       //this will hold the text describing the goal of the game
var Signature;                                                      //this will hold the text in the stats canvas with the Creators name
var difficultyMultiplier;                                           //this will hold the value multiplied to any points gained based on the difficulty
var newHighscoreAttained;                                           //this will be true if the score has passed the previous highscore, else false

var statsCanvas = {            //Game Stats Canvas
    canvas: document.createElement("canvas"),
    Start: function () {
        this.canvas.width = statsCanvasWidth;      
        this.canvas.height = statsCanvasHeight;
        this.canvas.id = "stats";
        this.context = this.canvas.getContext("2d");
        this.top = 0;
        this.left = 0;
        this.right = this.canvas.width;
        this.bottom = this.canvas.height;
    },
    DrawImage : function(image_arg, image_x, image_y, image_w, image_h) {
        var image = new Image();
        image.src = image_arg;
        image.onload = function() {
            this.context.drawImage(image, image_x, image_y, image_w, image_h);
        };
    },
    Clear: function() {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    },
    Delete: function(){
        $(this).remove();
    }
}

//this will create the stats canvas and will create the objects that are written in it
function CreateStatsCanvas(){
    statsCanvas.Start();
    statctx = statsCanvas.context;
    score = new Text("22px", "Consolas", "white", 20, 25, "text", "SCORE: ");
    round = new Text("22px", "Consolas", "white", 170, 25, "text", "ROUND: ");
    kills = new Text("22px", "Consolas", "white", 300, 25, "text", "KILLS: ");
    progress = new Text("22px", "Consolas", "white", 20, 50, "text", "PROGRESS: ");
    highscore = new Text("22px", "Consolas", "white", 20, 75, "text", "HIGHSCORE: ");
    goal = new Text("20px", "Consolas", "white", 20, 100, "text", "GOAL: ");
    ballCount = new Text("22px", "Consolas", "white", 20, 125, "text", "SHOTS LEFT: ");
    type = new Text("22px", "Consolas", "white", 20, 150, "text", "GAME TYPE: ");
    difficulty = new Text("22px", "Consolas", "white", 20, 175, "text", "DIFFICULTY LEVEL: ");
    Signature = new Text("30px", "Consolas", "radial-gradient(red, orange, yellow)", 15, 205, "text", "Created by Nicolas Latour");

    StatsUpdate();
}

//this will create an object with the specifications sent into it
function Text(size, font, color, x, y, type, half){
    this.type = type;
    this.size = size;
    this.font = font;
    this.x = x;
    this.y = y;
    this.color = color;
    this.half = half;
    this.text;

    this.Update = function(txt = "", ctx = statctx){                                        //this will change the text with the parameter that was sent it when called and draw the new text
        this.text = this.half + txt;
        ctx.font = this.size + " " + this.font;
        ctx.fillStyle = color;
        ctx.fillText(this.text, this.x, this.y);                
    }
}

//this will clear and update all of the textboxes values and redraw them
function StatsUpdate(){
    statsCanvas.Clear();
    score.Update(Math.ceil(currentScore));
    round.Update(currentRound);
    kills.Update(killCount);
    progress.Update(GetProgressText());
    highscore.Update(Math.ceil(GetHighScore()));
    type.Update(gameType);
    difficulty.Update(gameDifficulty);
    goal.Update(goalText);
    if(gameType != "Dodgeball")
        ballCount.Update(ballsLeft);
    Signature.Update();
}

function GetProgressText(){
    switch (gameType){
        case "Story": {          //lots of balls, lots of enemies
            return((totalNumberOfEnemies - killCount) + " enemies left"); 
        }
        case "Survival": {      //lots of balls, lots of enemies, no end in sight
            return((totalNumberOfEnemies - killCount) + " enemies left"); 
        }
        case "Horde": {         //1 ball per turn, lots of enemies, get to the fifth wave
            return((totalNumberOfEnemies - killCount) + " enemies left"); 
        }
        case "Dodgeball": {     //no enemies, lots of balls per turn
            return((dodgeballRoundGoal - currentRound) + " rounds left");
        }
    }
}

function GetHighScore(){
    switch (gameType){
        case "Story": {          //lots of balls, lots of enemies
            if(highScores.Story < currentScore){
                highScores.Story = currentScore;
                newHighscoreAttained = true;
            } 

            return highScores.Story;
        }
        case "Survival": {      //lots of balls, lots of enemies, no end in sight
            if(highScores.Survival < currentScore){
                highScores.Survival = currentScore;
                newHighscoreAttained = true;
            }

            return highScores.Survival;
        }
        case "Horde": {         //1 ball per turn, lots of enemies, get to the fifth wave
            if(highScores.Horde < currentScore){
                highScores.Horde = currentScore;
                newHighscoreAttained = true;
            }

            return highScores.Horde;
        }
        case "Dodgeball": {     //no enemies, lots of balls per turn
            if(highScores.Dodgeball < currentScore){
                highScores.Dodgeball = currentScore;
                newHighscoreAttained = true;
            }

            return highScores.Dodgeball;
        }
    }
}

function RoundBonusPoints(){
    currentScore += roundBonus;
}