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
   
## Original Flags
### Enable Step Mode: `--step`	
* Default: false
		
### Enable Periodic Behaviour: `--periodic`
* Default: false		
	
### Change Random Factor: `--random <Percentage of living cells in random seed>`  
   * e.g	`--random 0.5` sets the random factor to 50%
   * Random factor must be a floating point value between 0 and 1.
   * Default: 0.5

### Change Update Rate: `--max-update <Number of updates/second>`  
   * e.g. `--max-update 5` changes the update rate to 5 updates/second	
   * Can be a floating point value between 1 and 30.
   * Default: 5
   
   
### Change Number of Generations: `--generations <Positive whole number>`

   * e.g. `--generations 10` sets the number of generations to 10
   * Must be a positive whole number
   * Default: 50
   
### Input File: `--seed <File Path to a .seed file>` 
 
   * e.g. `--seed ".\exampleSeed.seed"` will set the initial conditions to those stored in exampleSeed.seed
   * Must be a valid file with the `.seed` extension
   * Default: N/A
   	
### Dimensions: `--dimensions <width> <height>`
  * e.g. `--dimensions 40 20` sets the dimensions to 40 columns wide and 20 rows tall.		
  * These values must be between 4 and 48 and must be whole numbers.
  * Default: 16x16

## Updated Flags

### Neighbourhood 

#### Usage: 
  	`--neighbour <type> <order> <centre-count>`

#### Defaults: 
    * The game will use a 1st order Moore neighbourhood that doesn’t count the centre.

  * #### Validation: 
    * The neighbourhood type must must be one of two strings, either "moore" or "vonNeumann", case insensitive. 
    * The order must be an integer between 1 and 10 (inclusive) and less than half of the
smallest dimensions (rows or columns). 
	* Whether the centre is counted will be a boolean (true or false)

### Survival and Birth
The number of live neighbours required for a cell to survive or be born in evolution may be specified using the --survival and --birth options respectively. These flag should be followed by an arbitrary number of parmeters (greater than or equal to 0).

#### Defaults: 
* Either 2 or 3 live neighbours are required for a cell to survive. Exactly 3 live neighbours are required for a cell to be born.

#### Validation: 
* Each parameter must be a single integer, or two integers separated by ellipses (. . . ).
* Integers separated by ellipses represent all numbers within that range (inclusively).
* The numbers provided must be less than or equal to the number of neighbouring cells and non-negative.

#### Usage: ``--survival <param1> <param2> <param3> ... --birth <param1> <param2> <param3> ...`

### Generational Memory
The number of generations stored in memory for the detection of a steady-state may be specified using the
–memory option. This flag should be followed by a single parameter.

#### Defaults: The program should store 16 generations for detecting a steady-state.

#### Validation: The value must be an integer between 4 and 512 (inclusive).

#### Usage: `--memory <number>`

### Output File
The path of the output file may be specified using the --output options. This flag should be followed by a
single parameter.

#### Defaults: No output file is used.

#### Validation: The value must be a valid absolute or relative file path with the .seed file extension.

#### Usage: `--output <filename>`

### Ghost Mode
Whether the program will render the game using ghost mode may be specified using the --ghost option.

#### Defaults: The program will not run in ghost mode

#### Validation: N/A

#### Usage: --ghost

  
## Notes 

* Flags can be inputted in any order with any combination
  * `dotnet run life.dll --step --generations 20 --periodic` would activate step conditions and periodic boundary conditions and set the number of generations to 20
* If any flag parameter is incorrectly input the default value will be used