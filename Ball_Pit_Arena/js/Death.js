var deathTick;                              //used to count the number of frames the game has passed for death animation
var grimClock;                              //death animation clock
var deathPosX;                              //this will hold the x position of the player death
var deathPosY;                              //this will hold the y position of the player death
var deathX;                                 //this will be the current x position of the death animation frame
var deathY;                                 //this will be the current y position of the death animation frame
var deathPic = new Image();                 //this will be the entire spritesheet of the death animation 
var dPSrc = "./spritesheets/explode.png";   //this will be the source for the spritesheet animation
var deathSound;                             //this will play when the character dies
var dSSrc = "./audio/explode.mp3";          //this will be the source for the death explosion sound
var dFramesPerCol;                          //this will say the number of frames per column in the death spritesheet
var dFramesPerRow;                          //this will say the number of frames per row in the death spritesheet
var dWidth;                                 //this will hold the width of the death spritesheet
var dHeight;                                //this will hold the height of the death spritesheet
var dFrameWidth;                            //this will be the width of an individual frame
var dFrameHeight;                           //this will be the height of an individual frame

//This will initialize the tick, set the interval for and load the values for the death animation
function DeathScene(){
    deathTick = 0;
    grimClock = setInterval(Death, 1000/fps*3);
    GetDeathPosition();
    deathSound.play();
}

//This initializes the values for the sprite animation for the death scene
function GetDeathPosition(){
    deathPosX = deathPosX - dFrameWidth/2;              //adjust the position to keep the death position at the centre of the frame
    deathPosY = deathPosY - dFrameHeight/2;             //adjust the position to keep the death position at the centre of the frame
}

//This is the draw method for the death animation
function Death(){
    ReDrawBoard();                                      //this erases the board

    //Add death animation
    deathX = (deathTick % dFramesPerCol) * dFrameWidth;
    deathY = Math.floor(deathTick / dFramesPerCol) * dFrameHeight;
    ctx.drawImage(deathPic, deathX, deathY, dFrameWidth, dFrameHeight, deathPosX, deathPosY, dFrameWidth, dFrameHeight);
    //(picture, frame pos x in picture, frame pos y in picture, frame width, frame height, death position in canvas (x,y), frame height and width)

    if(deathTick == dFramesPerCol*dFramesPerRow){        //end death animation
        StopGrimClock();
        DisplayFinalResults();
    }
    else
        deathTick++;
}

function StopGrimClock(){
    clearInterval(grimClock);
    grimClock = undefined;
}