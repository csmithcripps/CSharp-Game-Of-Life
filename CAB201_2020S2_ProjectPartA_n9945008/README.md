# Conway's Game of Life - .NET Console Implementation  
**author:** Cody Cripps - n9945008  
**date:** dd/mm/yyyy

## Build Instructions

### Build and Run with Visual Studio
1. Navigate to "CAB201_2020S2_ProjectPartA_n9945008\Life" (Specifically the upper level Life folder as there is a subfolder also titled life).
2. Open the Solution file (.sln) titled "Life.sln" with Visual Studio.
3. To build from VS either:  
  + *To build and Run from VS:* Press the button in the top ribbon with the green arrow labelled "Life"
  + *To build only:* Go to the build dropdown and click build

## Usage 
Navigate to "CAB201_2020S2_ProjectPartA_n9945008\Life\Life" in your console.

  * To run with default settings use `dotnet run life.dll`
  * Alternatively a number of flags can be added after this, these are as follows: 
   
   ---
   
#### Enable Step Mode: `--step`	
* Default: false
		
#### Enable Periodic Behaviour: `--periodic`
* Default: false		
	
#### Change Random Factor: `--random <Percentage of living cells in random seed>`  
   * e.g	`--random 0.5` sets the random factor to 50%
   * Random factor must be a floating point value between 0 and 1.
   * Default: 0.5

#### Change Update Rate: `--max-update <Number of updates/second>`  
   * e.g. `--max-update 5` changes the update rate to 5 updates/second	
   * Can be a floating point value between 1 and 30.
   * Default: 5
   
   
#### Change Number of Generations: `--generations <Positive whole number>`

   * e.g. `--generations 10` sets the number of generations to 10
   * Must be a positive whole number
   * Default: 50
   
#### Input File: `--seed <File Path to a .seed file>` 
 
   * e.g. `--seed ".\exampleSeed.seed"` will set the initial conditions to those stored in exampleSeed.seed
   * Must be a valid file with the `.seed` extension
   * Default: N/A
   	
#### Dimensions: `--dimensions <width> <height>`
  * e.g. `--dimensions 40 20` sets the dimensions to 40 columns wide and 20 rows tall.		
  * These values must be between 4 and 48 and must be whole numbers.
  * Default: 16x16

  
## Notes 

* Flags can be inputted in any order with any combination
  * `dotnet run life.dll --step --generations 20 --periodic` would activate step conditions and periodic boundary conditions and set the number of generations to 20
* If any flag parameter is incorrectly input the default value will be used