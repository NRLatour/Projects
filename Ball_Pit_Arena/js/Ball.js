//ball variables
var balls = [];                     //contains the balls
var ballColor = "Turquoise";	    //ball color
var ballSpeed;                      //this will be the speed of the balls
var ballRadius;                     //this will be the radius of the balls
var before;                         //this will keep track of the angle of the ball before checking if it hit any walls
var impact;                         //this will say whether the ball hit an object or not

class Ball{                                                     //these will be the balls shot from the player and the enemies
    constructor(x, y, angle){
        this.x = x;
        this.y = y;
        this.radius = ballRadius;
        this.angle = angle;
        this.speed = ballSpeed;
        this.health = 3;
        this.color = this.GetColor();
    }
    Slide(distance = this.speed){
        this.x += distance * Math.sin(this.angle);
        this.y += distance * Math.cos(this.angle);
        this.Draw();
    }
    Draw(){
        ctx.beginPath();
        ctx.arc(this.x, this.y, this.radius, 0, 2 * Math.PI);   //(x, y, radius, starting angle, end angle)
        ctx.fillStyle = this.color;
        ctx.fill();
    }
    Hits(){ //hit player, enemies or walls
        impact = false;;
        if(Hit(this, player)){ //hit the player
            player.Die();
            this.Die();
            impact = true;
        }
        else{
            for(var i = 0; i < pieces.length; i++){
                if(Hit(this, pieces[i])){ //hit other pieces
                    pieces[i].Die();         
                    this.Die();
                    impact = true;
                }
            }

            for(var i = 0 ; i < balls.length; i++){
                if(balls[i] != this){ //if it is a different ball
                    if(Math.abs(balls[i].x - this.x) < this.radius * 2 && Math.abs(balls[i].y - this.y) < this.radius * 2){ //check collision by proximity
                        balls[i].Die();
                        this.Die();
                        impact = true;
                    }
                }
            }

            this.HitWall();
        }

        if(impact)
		  RedrawAll();  
    }
    HitWall(){ //hit walls
        if(gameType == "Dodgeball")
            before = this.angle;

        if(this.x + this.radius > gameArea.right)
            this.angle = this.angle * -1;
        else if(this.x - this.radius < gameArea.left)                    
            this.angle = this.angle * -1;
        else if(this.y + this.radius > gameArea.bottom)                    
            this.angle = -Math.PI - this.angle;
        else if(this.y - this.radius < gameArea.top)                   
            this.angle = -Math.PI - this.angle;

        if(gameType == "Dodgeball" && before != this.angle){
            this.health--;
            this.color = this.GetColor();
        }
    }
    Die(){
        balls.splice(balls.indexOf(this), 1);
    }
    GetColor(){
        switch(this.health){
            case 3:{
                return ballColor;
            }
            case 2:{
                return "lightgreen";
            }
            case 1:{
                return "tomato";
            }
            case 0:{
                this.Die();
                break;
            }
        }
    }
}