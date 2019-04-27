var finalResult;
var ScoreDistribution;             	//this will be used for displaying the results
var gameAreaBackGround = "radial-gradient(red, yellow, green)";
var gradient;
var endResult;						//this will display whether they won or lost
var finalScore;						//this will hold the text of "Final Score"
var currentScoreDisplay;			//this will display the current score before adding the bonus points
var killDisplay;					//this will display the number of kills
var roundDisplay;					//this will display the number of rounds survived
var speedBonus;						//this will display the round bonus amount
var ballBonus;						//this will display the balls accumulated bonus
var winBonusDisplay;				//this will display the win bonus
var diffMultDisplay;				//this will display the difficulty multiplier
var totalScore;						//this will display the final total score
var resultPositions;               	//this will be used for displaying the results
var winResult;						//this will contain the text that will display at the end to say that the player won
var loseResult; 					//this will contain the text that will display at the end to say that the player lost
var tickLimit;						//this will be the limit for the draw animation
var resultsToDraw = [];				//this will contain the various displays that need to be drawn
var resultsToAdd = [];				//this will contain the results that will be progressively added
var nextDisplayY;					//this will be the y coordinate for the next results display
var yDisplayIncrement;				//this will be the increment between results displays
var displayX;						//this will be the x coordinate for the displays
var resultsSwitch = [];				//this will hold the text to determine the draw method to call
var addIndex;						//this will be the index of the current resultsToAdd/resultsSwitch object
var displays = [];					//this will hold the value of the second half of the display text for the results to add objects
var currentDisplay;					//this wll hold the value of the current display that is being drawn in the interval
var newHighscore;

function DisplayFinalResults(){
	rP = resultPositions;
	
	tick = 0;
	tickLimit = 100;
	gameTimer = setInterval(Dark, 1000/90);
	addIndex = 0;
    
	finalScore = new Text("32px", "Consolas", "white", rP.finalScore.x, rP.finalScore.y, "text", "Final Score");
	resultsToDraw.push(finalScore);
	SetNextDisplayY(rP.finalScore.y + 10);

	if(won){
		//Initialize the results gradient
    	CreateWinGradient();
		finalResult = winResult;
		endResult = new Text("50px", "Garamond", gradient, rP.endResult.x, rP.endResult.y, "text", finalResult);
		resultsToDraw.push(endResult);
		SetResultsToDraw();

		winBonusDisplay = GetTextToAdd("Win Bonus:  ", "winBonusDisplay");
    	resultsToAdd.push(winBonusDisplay);
	}
	else{
		finalResult = loseResult;
		endResult = new Text("38px", "Papyrus", "red", rP.endResult.x, rP.endResult.y, "text", finalResult);
		resultsToDraw.push(endResult);
		SetResultsToDraw();
	}

    diffMultDisplay  = GetTextToAdd("Difficulty:  ", "diffMultDisplay");
    resultsToAdd.push(diffMultDisplay);

    totalScore = new Text("24px", "Consolas", "white", displayX, nextDisplayY, "text", "Total Score:  ");
	SetNextDisplayY();
}

function CreateWinGradient(){    
    gradient = ctx.createLinearGradient(rP.endResult.x, rP.endResult.y, rP.endResult.x + 200, rP.endResult.y + 40);
    gradient.addColorStop("0.0", "lime");
    gradient.addColorStop("0.25", "yellow");
    gradient.addColorStop("0.5", "orange");
    gradient.addColorStop("0.75", "red");
    gradient.addColorStop("1.0", "magenta");
}

function CreateHighscoreGradient(){    
    gradient = ctx.createLinearGradient(rP.endResult.x - 25, nextDisplayY + 10, rP.endResult.x + 275, nextDisplayY + 40);
    gradient.addColorStop("0.0", "magenta");
    gradient.addColorStop("0.25", "red");
    gradient.addColorStop("0.5", "orange");
    gradient.addColorStop("0.75", "yellow");
    gradient.addColorStop("1.0", "lime");
}

function Dark(){
	if(tick < tickLimit){
		ctx.fillStyle = "black";
		ctx.globalAlpha = 0.04;
        ctx.fillRect(0, 0, gameArea.canvas.width, gameArea.canvas.height);
        tick++;
	}
	else{
		StopTimer();
		ctx.globalAlpha = 1;
		gameArea.canvas.style.background = "black";
		gameArea.Clear();

		SetDrawParameters(resultsSwitch[addIndex]);
	}
}

function SetResultsToDraw(){
	roundDisplay = GetTextToAdd("Rounds Survived:  ", "roundDisplay"); 
	resultsToAdd.push(roundDisplay);

	switch (gameType){
        case "Story": 
        case "Survival": 
        case "Horde": {         
        	killDisplay = GetTextToAdd("Kill Count:  ", "killDisplay");
        	resultsToAdd.push(killDisplay);

        	if(won){
	            speedBonus = GetTextToAdd("Speed Bonus:  ", "speedBonus");
	        	resultsToAdd.push(speedBonus);

	        	ballBonus = GetTextToAdd("Balls Left:  ", "ballBonus");
	        	resultsToAdd.push(ballBonus);
			}
            break;
        }
    }
}

function GetTextToAdd(left, name){
	temp =  new Text("24px", "Consolas", "white", displayX, nextDisplayY, "text", left);
	SetNextDisplayY();
	resultsSwitch.push(name);
	return temp;
}

function SetDrawParameters(display){			//adjust the time dependent on the tick limit
	tick = 0;
	switch(display){
		case "killDisplay":{
			tickLimit = killCount;
			break;
		}
		case "speedBonus":{
			tickLimit = totalNumberOfEnemies * 1.2 / pointsMult - currentRound;
			break;
		}
		case "ballBonus":{
			tickLimit = ballsLeft;
			break;
		}
		case "roundDisplay":{
			tickLimit = currentRound;
			break;
		}
		case "winBonusDisplay":{
			tickLimit = 0;		//only goes once
			break;
		}
		case "diffMultDisplay":{
			tickLimit = 0;		//only goes once
			break;
		}
	}

	time = GetDrawTime(tickLimit);
	currentDisplay = display;
	gameTimer = setInterval(DrawBonusDisplay, time);
}

function GetDrawTime(val){				//larger value means smaller time
	return (val == 0)? 100 : 600 / val * 1.2;
}

function SetNextDisplayY(y = nextDisplayY){
	nextDisplayY = y + 30;
}

function DrawBonusDisplay(){
	if(tick < tickLimit){
		IncrementBonus();
		GetTotal();
		DrawResults();
		tick++;
	}
	else{
		StopTimer();
		if(displays[addIndex] == undefined)
			IncrementBonus(false);
		
		DrawResults(true);
	}
}

function IncrementBonus(increment = true){
	switch(currentDisplay){
		case "killDisplay":{
			if(increment)
				ScoreDistribution.Kills++;
			displays[addIndex] = ScoreDistribution.Kills + " -> " + ScoreDistribution.Kills * killPoints + "pts";
			break;
		}
		case "speedBonus":{
			if(increment)
				ScoreDistribution.SpeedBonus++;
			displays[addIndex] = ScoreDistribution.SpeedBonus + " -> " + ScoreDistribution.SpeedBonus * roundSpeedBonus + "pts";
			break;
		}
		case "ballBonus":{
			if(increment)
				ScoreDistribution.BallsRemaining++;
			displays[addIndex] = ScoreDistribution.BallsRemaining + " -> " + ScoreDistribution.BallsRemaining * ballsAccumulatedBonus + "pts";
			break;
		}
		case "roundDisplay":{
			if(increment)
				ScoreDistribution.Rounds++;
			displays[addIndex] = ScoreDistribution.Rounds + " -> " + ScoreDistribution.Rounds * roundBonus + "pts";
			break;
		}
		case "winBonusDisplay":{
			displays[addIndex] = winBonus + "pts";
			ScoreDistribution.Total += winBonus;
			break;
		}
		case "diffMultDisplay":{			
			displays[addIndex] = "X" + pointsMult;
			ScoreDistribution.Total *= pointsMult;
			break;
		}
	}
}

function GetTotal(){
	ScoreDistribution.Total = (ScoreDistribution.Kills * killPoints + 
							ScoreDistribution.SpeedBonus * roundSpeedBonus + 
							ScoreDistribution.BallsRemaining * ballsAccumulatedBonus +
							ScoreDistribution.Rounds * roundBonus);
}

function DrawResults(next = false){
	gameArea.Clear();
	resultsToDraw.forEach(ResultsUpdate);					//display the hard coded displays

	for(var i = 0; i <= addIndex; i++){						//display all of the dependent scores or bonuses
		resultsToAdd[i].Update(displays[i], ctx);
	}

	totalScore.Update(ScoreDistribution.Total, ctx);		//display the total score

	if(ScoreDistribution.Total > currentScore){				//update the score stats if the total goes above the current score
		currentScore = ScoreDistribution.Total;
		StatsUpdate();
	}

	if(next){												//finished a display index
		if(resultsToAdd.length > addIndex + 1){				//check if there is another index to process
			addIndex++;
			SetDrawParameters(resultsSwitch[addIndex]);
		}
		else{												//the array has been fully processed
			if(newHighscoreAttained){	//the current score is the new highscore
				CreateHighscoreGradient();
				newHighscore = new Text("40px", "Consolas", gradient, rP.endResult.x - 25, nextDisplayY + 10, "text", "NEW HIGHSCORE!");
				newHighscore.Update("", ctx);
			}
			$("#start").attr("disabled", false);
		}
	}
}

function ResultsUpdate(item){
	item.Update("", ctx);
}