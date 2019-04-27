var fireworksSrc = {"blue": "spritesheets/blueshot.png",
					"red": "spritesheets/redshot.png",
					"violet": "spritesheets/violetshot.png",
					"yellow": "spritesheets/yellowshot.png"}
var blueFireworks;							//this will hold the spritesheet for the blue firework
var redFireworks;							//this will hold the spritesheet for the red firework
var violetFireworks;						//this will hold the spritesheet for the violet firework
var yellowFireworks;						//this will hold the spritesheet for the yellow firework
var fireworkTick;              				//used to count the number of frames the game has passed for fireworks animation
var fireworkClock;              			//fireworks animation clock
var fireworkPosX;                          	//this will hold the x position of the ball firework
var fireworkPosY;                           //this will hold the y position of the ball firework
var fireworkFrameX;                         //this will be the current x position of the firework animation frame in the spritesheet
var fireworkFrameY;                         //this will be the current y position of the firework animation frame in the spritesheet
var fireworkSound;                          //this will play when the ball explodes
var fSSrc = "./audio/explode.mp3";          //this will be the source for the firework explosion sound
var fFramesPerCol;                          //this will say the number of frames per column in the firework spritesheet
var fFramesPerRow;                          //this will say the number of frames per row in the firework spritesheet
var fWidth;                                 //this will hold the width of the firework spritesheet
var fHeight;                                //this will hold the height of the firework spritesheet
var fFrameWidth;                            //this will be the width of an individual frame
var fFrameHeight;                           //this will be the height of an individual frame							
var currentFireworks;						//this will hold the current spritesheet used for the fireworks

//This will initialize the tick, set the interval for and load the values for the death animation
function WinScene(){
	if(balls.length > 0){
    	GetFirework(balls[0]);
	}
	else{
		DisplayFinalResults();
	}
}

function GetFirework(ball){
	fireworkTick = 0;
    fireworkClock = setInterval(Fireworks, 1000/fps);
    GetRandomColor();
    GetFireworkPosition(ball);
    balls.shift();
}

function GetRandomColor(){
	num = Math.floor(Math.random() * 4);

	switch(num){
		case 0: {
			currentFireworks = blueFireworks;
			break;
		}
		case 1: {
			currentFireworks = redFireworks;
			break;
		}
		case 2: {
			currentFireworks = violetFireworks;
			break;
		}
		case 3: {
			currentFireworks = yellowFireworks;
			break;
		}
	}
}

//This initializes the values for the sprite animation for the death scene
function GetFireworkPosition(ball){
    fireworkPosX = ball.x - fFrameWidth/2;              //adjust the position to keep the death position at the centre of the frame
    fireworkPosY = ball.y - fFrameHeight/2;             //adjust the position to keep the death position at the centre of the frame
}

//This is the draw method for the death animation
function Fireworks(){
    RedrawAll();

    //Add death animation
    fireworkFrameX = (fireworkTick % fFramesPerCol) * fFrameWidth;
    ctx.drawImage(currentFireworks, fireworkFrameX, fireworkFrameY, fFrameWidth, fFrameHeight, fireworkPosX, fireworkPosY, fFrameWidth, fFrameHeight);
    //(picture, frame pos x in picture, frame pos y in picture, frame width, frame height, position in canvas (x,y), frame height and width)

    if(fireworkTick == fFramesPerCol*fFramesPerRow){        //firework animation death animation
        clearInterval(fireworkClock);
        if(balls.length > 0){
	    	GetFirework(balls[0]);
		}
		else{
			RedrawAll();
			DisplayFinalResults("You Won!");
		}
    }
    else
        fireworkTick++;
}