var myCanvas = document.getElementById("myCanvas");
myCanvas.width = 200;
myCanvas.height = 100;
  
var ctx = myCanvas.getContext("2d");

// to draw a bar chart requires nowing how to draw:
// a line 
// and filling in those bars 

function drawLine
(ctx, startX, startY, endX, endY,color){
    ctx.save();
    ctx.strokeStyle = color;
    ctx.beginPath(); // this is how a line is drawn
    ctx.moveTo(startX,startY); // this sets the starting point 
    ctx.lineTo(endX,endY); // indicates the end point
    ctx.stroke(); // this starts the actuals drawing 
    ctx.restore();
}

// helper function for filling in the bars:

function drawBar
(ctx, upperLeftCornerX, upperLeftCornerY, width, height,color){
    ctx.save();
    ctx.fillStyle=color;
    ctx.fillRect(upperLeftCornerX,upperLeftCornerY,width,height);
    ctx.restore();
}

// --------------- end of helper functions--------------------


// so now let use create a set model for now. 
// to fill the bar chart.

var myVinyls = {
    "Class": 1,
    "Alternative rock": 14,
    "Pop": 2,
    "Jazz": 12,
    "Class1": 1,
    "Alt": 14,
    "Pop1": 2,
    "Jazz1": 12
};


// implementing the bar chart component:
var Barchart = function(options){
    this.options = options; // these are storing the pased parameters to the class
    this.canvas = options.canvas;
    this.ctx = this.canvas.getContext("2d");
    this.colors = options.colors;
  
    // next part is the draw function 
    this.draw = function(){
        var maxValue = 0;
        for (var categ in this.options.data){
            // getting the maximum value for the data model 
            // so that the charts stays withing the display area.
            maxValue = Math.max(maxValue,this.options.data[categ]);
        }
        // stores the hieght 
        var canvasActualHeight 
        = this.canvas.height - this.options.padding * 2;
        // stores the width
        var canvasActualWidth 
        = this.canvas.width - this.options.padding * 2; // the paddding is the space between 
        // the edge of the canvas and chart within.
 
        //drawing the grid lines

        var gridValue = 0;

        while (gridValue <= maxValue){
            var gridY = 
            canvasActualHeight * (1 - gridValue/maxValue) + this.options.padding;
            drawLine(
                this.ctx,
                0,
                gridY,
                this.canvas.width,
                gridY,
                this.options.gridColor
            );
             
            //writing grid markers
            this.ctx.save();
            this.ctx.fillStyle = this.options.gridColor;
            this.ctx.font = "bold 10px Arial";
            this.ctx.fillText(gridValue, 10,gridY - 2);
            this.ctx.restore();
 
            gridValue+=this.options.gridScale;
        }
  
        //drawing the bars
        var barIndex = 0;
        var numberOfBars = Object.keys(this.options.data).length;
        var barSize = (canvasActualWidth)/numberOfBars;
 
        for (categ in this.options.data){
            var val = this.options.data[categ];
            var barHeight = Math.round( canvasActualHeight * val/maxValue) ;
            drawBar(
                this.ctx,
                this.options.padding + barIndex * barSize,
                this.canvas.height - barHeight - this.options.padding,
                barSize = 10,
                barHeight,
                this.colors[barIndex%this.colors.length]
            );

            barIndex++;
        }
        //drawing series name
        this.ctx.save();
        this.ctx.textBaseline="bottom";
        this.ctx.textAlign="center";
        this.ctx.fillStyle = "#000000";
        this.ctx.font = "11px Arial";
        this.ctx.fillText(this.options.seriesName, this.canvas.width/2,this.canvas.height);
        this.ctx.restore();  
    }
}
var myBarchart = new Barchart(
    {
        canvas:myCanvas,
        seriesName:"Vinyl records",
        padding:20,
        gridScale:5,
        gridColor:"#eeeeee",
        data:  myVinyls,
        colors:["#a55ca5","#67b6c7", "#bccd7a","#eb9743"]
    }
);
//myBarchart.draw();


var LineChart = function(options)
{

    this.options = options; // these are storing the pased parameters to the class
    this.canvas = options.canvas;
    this.ctx = this.canvas.getContext("2d");
    this.colors = options.colors;

}


document.getElementById("myCanvas").innerText= myBarchart.draw();