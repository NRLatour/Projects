//key codes
key = {left:37, up: 38, right: 39, down: 40, enter: 13, esc: 27, w: 87, a: 65, s: 83, d: 68}

//board variables
var boardDimensions;        //it will be a 16 x 16 game board
var padding;                //padding between squares
var tileSize;               //size of squares on board
var gameCanvasSize;         //dimensions of board
var statsCanvasHeight;      //height of stats canvas
var statsCanvasWidth;       //width of stats canvas
var ctx;                    //gameArea context
var board = [];             //contains tiles on the board
var nextTile;               //space between tile starting coordinates (tileSize + padding)
var rightLimit;             //This will be the column limit for the player on the right side
var leftLimit;              //This will be the column limit for the player on the left side
var topLimit;               //This will be the row limit for the player on the top
var bottomLimit;            //This will be the row limit for the player on the bottom

class Tile{					//These will be the tiles to make up the board on the canvas
    constructor(x, y, color, row, col){
        this.x = x;
        this.y = y;
        this.width = tileSize;
        this.height = tileSize;
        this.color = color;
        this.row = row;
        this.col = col;
        this.content = null;
    }
    HasAPiece(){
        return(this.content != null);
    }
    Draw() {
        if(!OutOfBounds(this.row, this.col)){                                           //this will not draw squares that are out of bounds for the dodgeball game
            ctx = gameArea.context;
            ctx.fillStyle = this.color;
            ctx.fillRect(this.x, this.y, this.width, this.height);
        }
    }
    SetPiece(obj){
        this.content = obj;
        this.content.EndPosition(this);
        this.content.Draw();
    }
    LosePiece(){
        this.content = null;
    }
    TakePiece(){
        var c = this.content;
        this.LosePiece();
        return c;
    }
    CopyPiece(){
        return this.content;
    }
}

function OutOfBounds(r, c){
    return(gameType == "Dodgeball" && (r == topLimit - 1 || r == bottomLimit || c == rightLimit || c == leftLimit - 1));
}

function DrawTile(obj){ //used in foreach methods for drawing tiles
    obj.Draw();
}

function CreateBoard(){ //create tiles and use the to Draw the board
    canvasBorder = gameCanvasSize - nextTile;
    row = 0; //assigns the row of the tile in the board array
    col = 0; //assigns the column of the tile in the board array
    for(var y = padding; y <= canvasBorder; y += (tileSize + padding), row++){
        board[row] = [];
        for(var x = padding; x <= canvasBorder; x += (tileSize + padding), col++){
            board[row][col] = (new Tile(x, y, GetTileColor(col, row), row, col));
            board[row][col].Draw();
        }
        col = 0;
    }
}

function GetTileColor(x, y){ //gets the alternating color of the tiles when creating the tiles
    if(y % 2 == 0)
        if(x % 2 == 0)
            return "white";                
        else
            return "black";
    else
        if(x % 2 == 0)
            return "black";                
        else
            return "white";              
}

function ReDrawBoard(){ //Draw each tile that was created
    gameArea.Clear();
    for (var y = 0; y < board.length; y++) {
        for(var x = 0; x < board[y].length; x++){
            board[y][x].Draw();
        }
    }
}

function CreatePlayer(){ //creates the player and assigns his tile space
    player = new Character(playerSize, playerColor);
    board[playerStartRow][playerStartCol].SetPiece(player);
}

function GenerateEnemy(num){
    spawns = (num + enemiesSpawned > totalNumberOfEnemies)? totalNumberOfEnemies - enemiesSpawned : num;    //ensure the number of spawns does not go above the total number limit
    var side1;
    var side2;
    var xquadrant;
    var yquadrant;

    if(spawns % 2 == 1 && spawns > 1){
        side1 = Math.floor(Math.random() * (Math.floor(spawns / 2) + 1)) + 1;
        side2 = spawns - side1;
    }
    else{
        side1 = Math.ceil(spawns / 2);
        side2 = spawns / 2;
    }

    sides = ["top", "right", "bottom", "left"];

    switch(Math.floor(Math.random() * 2)){
        case 0:{
            if(player.row < boardDimensions / 2){                       //player on top end of the gameboard
                SpawnEnemy(side1, sides, 2);              //spawn at the bottom
                if(player.col < boardDimensions / 2)                    //player is on the left side of the gameboard
                    SpawnEnemy(side2, sides, 1);          //spawn at the right
                else                                                    //player is on the right side of the gameboard
                    SpawnEnemy(side2, sides, 3);          //spawn at the left
            }
            else{                                                       //player on bottom end of the gameboard
                SpawnEnemy(side1, sides, 0);              //spawn at the top
                if(player.col < boardDimensions / 2)                    //player is on the left side of the gameboard
                    SpawnEnemy(side2, sides, 1);          //spawn at the right
                else                                                    //player is on the right side of the gameboard
                    SpawnEnemy(side2, sides, 3);          //spawn at the left                
            }
            break;
        }
        case 1: {
            if(player.col < boardDimensions / 2){                        //player is on the left side of the gameboard
                SpawnEnemy(side1, sides, 1);               //spawn at the right
                if(player.row < boardDimensions / 2)                     //player on top end of the gameboard
                    SpawnEnemy(side2, sides, 2);           //spawn at the bottom
                else                                                     //player on bottom end of the gameboard
                    SpawnEnemy(side2, sides, 0);           //spawn at the top
            }
            else{                                                        //player is on the right side of the gameboard
                SpawnEnemy(side1, sides, 3);               //spawn at the left
                if(player.row < boardDimensions / 2)                     //player on top end of the gameboard
                    SpawnEnemy(side2, sides, 2);           //spawn at the bottom
                else                                                     //player on bottom end of the gameboard
                    SpawnEnemy(side2, sides, 0);           //spawn at the top
            } 
            break;
        }
    }
}

function SpawnEnemy(num, arr, index){
    var x;
    var y;
    enemiesSpawned+= num;

    switch(arr[index]){
        case "left": {
            x = 0;
            for(var i = 0; i < num; i++){
                pieces.push(new Character(characterSize, enemyColor));  

                y = Math.floor(Math.random() * (boardDimensions - 2)) + 1;

                while(board[y][x].HasAPiece()){
                    y = Math.floor(Math.random() * (boardDimensions - 2)) + 1;
                }

                board[y][x].SetPiece(pieces[pieces.length - 1]);
            }
            break;
        }
        case "right": {
            x = boardDimensions - 1;
            for(var i = 0; i < num; i++){
                pieces.push(new Character(characterSize, enemyColor));  

                y = Math.floor(Math.random() * (boardDimensions - 2)) + 1;

                while(board[y][x].HasAPiece()){
                    y = Math.floor(Math.random() * (boardDimensions - 2)) + 1;
                }

                board[y][x].SetPiece(pieces[pieces.length - 1]);
            }
            break;
        }
        case "bottom": {
            y = boardDimensions - 1;
            for(var i = 0; i < num; i++){
                pieces.push(new Character(characterSize, enemyColor));  

                x = Math.floor(Math.random() * (boardDimensions - 2)) + 1;

                while(board[y][x].HasAPiece()){
                    x = Math.floor(Math.random() * (boardDimensions - 2)) + 1;
                }

                board[y][x].SetPiece(pieces[pieces.length - 1]);
            }
            break;
        }
        case "top": {
            y = 0;
            for(var i = 0; i < num; i++){
                pieces.push(new Character(characterSize, enemyColor));  

                x = Math.floor(Math.random() * (boardDimensions - 2)) + 1;

                while(board[y][x].HasAPiece()){
                    x = Math.floor(Math.random() * (boardDimensions - 2)) + 1;
                }

                board[y][x].SetPiece(pieces[pieces.length - 1]);
            }
            break;
        }
    }    
}