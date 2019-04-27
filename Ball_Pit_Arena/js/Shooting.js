var canvasClickBalanceX;                    //the click event gets the coordinates based on the page not the canvas, the canvas is offset by the centering
var canvasClickBalanceY;                    //the click event gets the coordinates based on the page not the canvas, the canvas is offset by the centering
var shootAngle;                     		//this will hold the x and y coordinates of the click to shoot the ball
var ballsLeft;				        		//keeps track of the number of shots available to the player per turn
var shotsPerTurn;			        		//keeps track of the number of shots available to the player per turn
var enemyBalls = [];                		//this will hold an array of balls that need to be moved by the enemy
var safeDistance;                   		//this will be the distance where the ball is guaranteed not to impact the one that shot it
var ballEndPosition = [];					//this will be the end position of the balls 
var shootAnimationTickLimit;				//this will be the tick limit for the ball animation when shot
var shootAnimationSafeDistDenom;			//this will be the fraction of each movement of the balls
var shootAnimationFrameSpeed;				//this will be the speed of the ball animation intervals
var playerBallTick;							//this will be the counter for the player shoot animation
var enemyBallTick; 							//this will be the counter for the enemy shoot animation
var playerBallTimer;						//this will be the timer for the player shoot animation
var enemyBallTimer; 						//this will be the timer for the enemy shoot animation
var activeAnimations;						//this will hold the number of active animations


function AdjustOffset(){                                    //this will help adjust the offset for the click on the canvas to aim properly
    canvasClickBalanceY = -gameArea.canvas.offsetTop;
    canvasClickBalanceX = -gameArea.canvas.offsetLeft;
}

function RemovePieces(){									//remove the pieces from the board
    pieces.forEach(function(e){board[e.row][e.col].LosePiece();});              
    pieces = [];
    enemiesSpawned = 0;
    RedrawAll();
}

function ShootPlayer(enemy){                            //calculate the angle to shoot at player from enemy position and shoot ball
    angle = GetShootAngle(player.centerx, player.centery, enemy);
    Shoot(enemy, (-angle + Math.PI/2), true);
}

function UpdateShots(){                                 //update the click (to shoot) if the number of available shots is lower than 1
	//update text box of shots
	ToggleClickEvent(ballsLeft >= 1);       
    StatsUpdate();          
}

function ToggleClickEvent(v){       //allow or disallow the shooting option on the canvas
    gameArea.Click(v);
}

function PlayerShoot(e){                    				//gets the click coordinates, the angle of the shot and calls to create call
    if(shootAnimation)
    	ToggleClickEvent(false);
	AdjustOffset();
    clickX = event.clientX + canvasClickBalanceX;           //gets the x coordinate of the mouse click
    clickY = event.clientY + canvasClickBalanceY;           //gets the y coordinate of the mouse click
    angle = GetShootAngle(clickX, clickY, player);
	Shoot(player, (-angle + Math.PI/2));                    //adjust the angle to work properly
}

function GetShootAngle(x, y, obj){                  //returns the angle between the center of the object and the mouse click coordinates
    adj = (x - (obj.x + obj.width/2));              //gets the adjacent right triangle side length for calculating the angle
    opp = (y - (obj.y + obj.height/2));             //gets the opposing right triangle side length for calculating the angle
    return angle = Math.atan2(opp, adj);            //use arcTan to get the angle between the object and the click
}

function Shoot(obj, angle, enemy = false){             //create the ball, move it to a safe distance and draw it
    opp = obj.height / 2;
    adj = obj.width / 2;
    safeDistance = Math.ceil(Math.sqrt(adj * adj + opp * opp) + obj.width / 2); //pythagore will give a minimum safe distance for the ball from the object to avoid self-destruction
    ball = new Ball(obj.x + adj, obj.y + opp, angle);               //generate the ball from the center of the shooting object with its angle
    
    balls.push(ball);                                               //add the ball to the balls array

    if(shootAnimation){
	    if(enemy){      
	        enemyBalls.push(ball);
	    }
	    else if(moving){
		    ball.Slide(safeDistance);                               //move the ball to a safe distance before drawing it
		}
		else{
			SlideBalls();
		}
	}
	else{
		ball.Slide(safeDistance);
	}

    if(obj == player){
        ballsLeft--;
        if(!shootAnimation)
        	UpdateShots();
    }
}

function SlideBalls(enemy = false){
	activeAnimations++;														//signal the beginning of an animation
	BallAnimationActive();
	if(enemy){		
		enemyBallTick = 0;		
		enemyBallTimer = setInterval(SlideEnemyBalls, shootAnimationFrameSpeed);
	}
	else{		
		playerBallTick = 0;
		playerBallTimer = setInterval(SlidePlayerBall, shootAnimationFrameSpeed);
	}
}

function SlideEnemyBalls(){
	if(enemyBallTick < shootAnimationTickLimit){
		enemyBalls.forEach(MoveBall);										//move every ball shot by the enemies
		RedrawAll();
		enemyBallTick++;
	}
	else{
		StopEnemyTimer();
		enemyBalls.forEach(BallCollisions);									//check for collisions and redraw
		enemyBalls = [];
		RedrawAll();
		activeAnimations--;													//signal the end of an animation
		BallAnimationActive();
	}
}

function SlidePlayerBall(){
	if(playerBallTick < shootAnimationTickLimit){
		MoveBall(balls[balls.length - 1], playerBallTick);					//the last ball shot was the players
		RedrawAll();
		playerBallTick++;
	}
	else{
		StopPlayerTimer();
		BallCollisions(balls[balls.length - 1]);							//check for collisions and redraw
		RedrawAll();
		activeAnimations--;													//signal the end of an animation
		BallAnimationActive()
	}
}

function StopEnemyTimer(){
	clearInterval(enemyBallTimer);
    enemyBallTimer = undefined;     
}

function StopPlayerTimer(){
	clearInterval(playerBallTimer);
    playerBallTimer = undefined;     
}

function MoveBall(ball){
	var counter;
	if(enemyBalls.length > 0)
		counter = enemyBallTick
	else
		counter = playerBallTick;

	slide = safeDistance * (shootAnimationTickLimit - counter) / shootAnimationSafeDistDenom;
	ball.Slide(slide);
}

function BallCollisions(ball){
	ball.Hits();
}

function BallAnimationActive(){
	if(activeAnimations > 0){
		DisableMoveEvent(true);												//disable movement while the player shooting animation is active
		ToggleClickEvent(false);											//disable the shoot event listener
	}
	else{
		MovingAnimationActive(false);										//enable movement while the player shooting animation is active
		UpdateShots();														//enable the shoot event listener
	}
}