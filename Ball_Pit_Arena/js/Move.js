//move variables
var speed;                  //speed moved per frame
var tick;                   //used to count the number of frames the game has passed for movement
var fps;                    //number of frames per second for the level
var enemyEndTiles = [];     //this will hold all of the end tiles for the enemies to avoid multiple enemies ending on the same tile
var gameTimer;              //this will hold the timer for the draw method
var moveTime;               //the number of ticks that the movement animation will take
var drawPlayer;             //whether the player is drawn or not
var moving;                 //this will determine if the player is moving

function Hit(ball, obj){
    //check if the circle is beyond the bounds of the rectangle
    if(ball.y + ball.radius < obj.top)                                  //ball is higher than the top of the obj (no possible collision)
        return false;
    else if(ball.y - ball.radius > obj.bottom)                          //ball is lower than the bottom of the obj (no possible collision)
        return false;
    if(ball.x + ball.radius < obj.left)                                 //ball is further left than the obj (no possible collision)
        return false;
    else if(ball.x - ball.radius > obj.right)                           //ball is further right than the obj (no possible collision)
        return false;
    
    var cX;                                                                 //this will be the closest x value to the ball that is on the rectangle
    var cY;                                                                 //this will be the closest y value to the ball that is on the rectangle

    if(ball.y <= obj.top)                                               //the top is the closest x value to the circle
        cY = obj.top;    
    else if(ball.y  >= obj.bottom)                                      //the bottom is the closest x value to the circle
        cY = obj.bottom;
    else                                                                //the y value of the ball is the closest x value in the rectangle to the circle
        cY = ball.y;

    if(ball.x <= obj.left)                                              //the left is the closest y value to the circle
        cX = obj.left;    
    else if(ball.x  >= obj.right)                                       //the right is the closest y value to the circle
        cX = obj.right;
    else                                                                //the y value of the ball is the closest y value in the rectangle to the circle
        cX = ball.x;

    dX = Math.abs(cX - ball.x);
    dY = Math.abs(cY - ball.y);

    return (dX * dX + dY * dY < ball.radius * ball.radius);
}

function ScatterShots(ball){
    ball.Slide();
    ball.Hits();        //move and check hits for ball sent in
}

function GameDraw(){ //this is the main game timer that will move every character and ball
    if(tick < moveTime){                
        ReDrawBoard();
        DrawPlayer();
        pieces.forEach(function Sled(item){item.Slide();});
        balls.forEach(ScatterShots);
        tick++;
    }
    else{
        StopTimer();        						          //stop calling this function

        ReDrawBoard();

        SetPieces();        						          //put pieces in their end tiles
        balls.forEach(function draw(item){item.Draw()});	  //draw all of the balls

        currentRound++;
        PlayerTurn();									      //start players turn        
    }   
}

function DrawPlayer(){
    if(drawPlayer)        
        player.Slide();
}

function HitsObject(item){
	item.Hits();
}

function SetPieces(){
	enemyEndTiles = [];				//empty the end tiles array to start new turn
    if(drawPlayer)                  //if the player is not on the board anymore, don't set
        Set(player);
    pieces.forEach(Set);
}

function Set(item){
    item.end.SetPiece(item);
}

function SetEnemyDirections(){//if the player moved, move enemies closer to player in order of the closest enemy
    pieces.sort(function (a,b){a.PlayerDistance()-b.PlayerDistance()}); //sort by closest to the player
    pieces.forEach(MoveEnemyPiece);
}

function EnemyStay(){
    pieces.forEach(function(obj){
        obj.SetDirection(board[obj.row][obj.col], board[obj.row][obj.col]);
    });
}

function MoveEnemyPiece(item){ //move the enemy piece closer to the player
    change = GetEnemyDirection(item, player); //get the directions to the player
    tile1 = board[item.row][item.col];
    tile2 = board[item.row + change.row][item.col + change.col];

    if(enemyEndTiles.includes(tile2)) //if the space is taken don't move, else move
        endTile = tile1;    
    else
    	endTile = tile2;    

    item.SetDirection(tile1, endTile);
    enemyEndTiles.push(endTile);
    tile1.LosePiece();
}

function GetEnemyDirection(enemy, player){ //checks the position of the player, which direction is longer (x or y) and returns an object with the {y,x} change for the movement
    if(Math.abs(enemy.row - player.row) >= Math.abs(enemy.col - player.col))
        if(enemy.row > player.row)
            return {row:-1, col:0}; //[y,x]
        else
            return {row:1, col:0};            
    else
        if(enemy.col > player.col)
            return {row:0, col:-1};                
        else
            return {row:0, col:1};                       
}

function CheckSpace(y1, x1, y2, x2){                                                    //checks the space that the unit wants to move to and sets the direction if it is free
    if(y2 >= bottomLimit || y2 < topLimit || x2 >= rightLimit || x2 < leftLimit){
        //alert("Off the board movement"); Can't move into wall
        return false;
    }
    else {
        tile1 = board[y1][x1];
        tile2 = board[y2][x2];
        
        if(tile2.HasAPiece())
            return false;
        else{
            piece = tile1.TakePiece();
            piece.SetDirection(tile1, tile2);
            return true;
        }
    }
}