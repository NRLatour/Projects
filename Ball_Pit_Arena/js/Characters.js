//player variables
var playerStartRow;                 //the starting row of the player
var playerStartCol;                 //the starting column of the player
var playerSize;                     //player sizes
var characterSize;                  //character sizes
var pieces = [];                    //contains the enemy pieces on the board
var player;                         //the variable holding the player character
var end;                            //value of tile that the moving piece will end in
var playerColor = "LimeGreen";      //this will be the color of the player character
var enemyColor = "Tomato";          //this will be the color of the enemy character
var spawnRate;                      //the number of enemies that will spawn every two turns
var shootingEnemies;                //whether the enemies shoot at the player or not
var enemiesSpawned;                 //this will keep track of the number of enemies that have been spawned

class Character{                    //these are the pieces on the board, player and enemies
    constructor(size, color){
        this.x;                     //this is the x position of the character
        this.y;                     //this is the y position of the character
        this.centerx;               //this is the center x position of the character
        this.centery;               //this is the center y position of the character
        this.width = size;          //this is the width of the character
        this.height = size;         //this is the height of the character
        this.color = color;         //this will hold the color of the character
        this.left;                  //this will hold the x value of the left side of the character
        this.right;                 //this will hold the x value of the right side of the character
        this.top;                   //this will hold the y value of the top side of the character
        this.bottom;                //this will hold the y value of the bottom side of the character
        this.row;                   //this will be the row position on the gameArea board that the piece is in
        this.col;                   //this will be the column position on the gameArea board that the piece is in
        this.dirx;                  //this will hold the direction value for the x axis movement (1 goes right, -1 -> left and 0 stay at x position)
        this.diry;                  //this will hold the direction value for the y axis movement (1 goes down, -1 -> up and 0 stay at y position)
        this.end;                   //this will be the board tile that the piece will end its movement on
        this.start;                 //this will be the board tile that the piece will begin its movement from
    }
    Draw() {                        //this will draw the character into the gameArea canvas
    	ctx = gameArea.context;
        ctx.fillStyle = this.color;
        ctx.fillRect(this.x, this.y, this.width, this.height);
    }
    EndPosition(tile = this.end){ //snap block into center of the tile that it finishes its movement into
        this.x = (tile.x + (tile.width - this.width)/2);
        this.y = (tile.y + (tile.height - this.height)/2);
        this.row = tile.row;
        this.col = tile.col;
        this.left = this.x;
        this.right = this.x + this.width;
        this.top = this.y;
        this.bottom = this.y + this.height;
        this.centerx = this.x + this.width/2;
        this.centery = this.y + this.height/2;
        this.end = null;                        //erase end tile value after setting it
    }
    PlayerDistance(){           //this will return the number of squares the piece needs to travel to reach the player
        return Math.abs(this.row - player.row) + Math.abs(this.col - player.col);
    }
    SetDirection(tile1, tile2){
        this.dirx = tile2.x - tile1.x;
        this.dirx = (this.dirx == 0)? 0: this.dirx / Math.abs(this.dirx); //avoid divide by 0
        this.diry = tile2.y - tile1.y;
        this.diry = (this.diry == 0)? 0: this.diry / Math.abs(this.diry);//avoid divide by 0
        this.end = tile2;
        this.start = tile1;
    }
    Slide(){                    //move the piece in the direction that was set and redraw
        this.x += this.dirx * speed;                                            
        this.y += this.diry * speed;
        this.left = this.x;
        this.right = this.x + this.width;
        this.top = this.y;
        this.bottom = this.y + this.height;
        this.centerx = this.x + this.width/2;
        this.centery = this.y + this.height/2;
        this.Draw();        
        if(this != player)      //Check if the piece killed the player
            this.KillPlayer();
    }
    KillPlayer(){
        if(this.left >= player.left && this.left <= player.right){                //left within x bounds of player
            if(this.top >= player.top && this.top <= player.bottom)               //top within y bounds of player (top left corner in player)
                player.Die();
            else if(this.bottom >= player.top && this.bottom <= player.bottom)    //bottom within y bounds of player (bottom left corner in player)
                player.Die();           
            else if(Math.abs(this.centerx - player.centerx) <= this.width/2 + player.width/2 && Math.abs(this.centery - player.centery) <= this.height/2 + player.height/2)
                player.Die();                                                     //the distance from the center of the character and player is less than the sum of half of their widths or heights
        }
        else if(this.right >= player.left && this.right <= player.right){         //right within x bounds of player
            if(this.top >= player.top && this.top <= player.bottom)               //top within y bounds of player (top right corner in player)
                player.Die();
            else if(this.bottom >= player.top && this.bottom <= player.bottom)    //bottom within y bounds of player (bottom right corner in player)
                player.Die();        
            else if(Math.abs(this.centerx - player.centerx) <= this.width/2 + player.width/2 && Math.abs(this.centery - player.centery) <= this.height/2 + player.height/2)
                player.Die();                                                     //the distance from the center of the character and player is less than the sum of half of their widths or heights
        }
        else if(Math.abs(this.centerx - player.centerx) <= this.width/2 + player.width/2 && Math.abs(this.centery - player.centery) <= this.height/2 + player.height/2)
                player.Die();                                                     //the distance from the center of the character and player is less than the sum of half of their widths or heights
        
    }
    Die(){
        var lost = false;

        if(this == player) //Check if the player just died    
            lost = true;                
        else{ //an enemy just died
            killCount++;
            pieces.splice(pieces.indexOf(this), 1);             //remove the piece from the 
            //add points to score
            currentScore += killPoints;
            StatsUpdate();

            CheckWin();
        }

        if(lost){
            drawPlayer = false;
            DisableMoveEvent(true);
            ToggleClickEvent(false);           
            deathPosX = this.centerx;
            deathPosY = this.centery;
            setTimeout(Lose, 0);
        }
    }         
}    