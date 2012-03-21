using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenAlg___Coin
{
    class GenAlg
    {
        const int numberOfVariables = 8;
        static int generations;
        static int numOfCoins;
        static double target;
        static int popSize;
        static int numOfChildren;
        static int tournamentSize;
        static int eliteSize;
        static int mutationRate;
        static int survivorCount;
        static List<string> population = new List<string>();
        static List<double> values = new List<double>();
        static List<double> fitness = new List<double>();
        static List<string> survivors = new List<string>();
        static Random rand = new Random();

        /*
         * This function is designed to handle multiple different variables for input.
         * Since there's 8 variables right now, and they are either ints or doubles,
         * having 8 small fucntions seems silly when it can be changed into 1 simpler
         * function.
         */
        static void getInput(int index)
        {
            string input;
            int tempInt = -1;
            double tempDouble = -1.00;
            bool validEntry = false;
            char inputType = 'i';
            while (!validEntry)
            {
                /*
                 * Output general starting text
                 */
                switch (index)
                {
                    case 0: //generations
                        Console.Write("Enter the number of generations: ");
                        inputType = 'i';
                        break;
                    case 1: //number of coins
                        Console.Write("Enter the number of coins in the system: ");
                        inputType = 'i';
                        break;
                    case 2: //target value
                        Console.Write("Enter the target value for the coins: ");
                        inputType = 'd';
                        break;
                    case 3: //population size
                        Console.Write("Enter the population size: ");
                        inputType = 'i';
                        break;
                    case 4: //number of children
                        Console.Write("Enter the number of children (50% - 100% of population size): ");
                        inputType = 'i';
                        break;
                    case 5: //tournament size
                        Console.Write("Enter tournament size (lower is better): ");
                        inputType = 'i';
                        break;
                    case 6: //elitist 
                        Console.Write("Enter the elitist percentage: ");
                        inputType = 'i';
                        break;
                    case 7: //mutation rate
                        Console.Write("Enter the mutation rate (percentage): ");
                        inputType = 'i';
                        break;
                } //end switch

                /*
                 * Get text, convert to either an int or a double based on what type is needed
                 */
                input = Console.ReadLine();
                if (inputType == 'i')
                {
                    try
                    {
                        tempInt = Convert.ToInt32(input);
                    }
                    catch
                    {
                        Console.WriteLine("Please try entry again.");
                    }
                }
                else
                {
                    try
                    {
                        tempDouble = Convert.ToDouble(input);
                    }
                    catch
                    {
                        Console.WriteLine("Please try entry again.");
                    }
                }
                /*
                 * Check if there was a value entered, and set the appropriate 
                 * variable to the converted value
                 */
                if (tempInt >= 0 || tempDouble >= 0)
                {
                    validEntry = true;
                    switch (index)
                    {
                        case 0: //generations
                            generations = tempInt;
                            if (generations <= 0)
                            {
                                validEntry = false;
                                Console.WriteLine("Generations must be greater than 0.");
                            }
                            break;
                        case 1: //number of coins
                            numOfCoins = tempInt;
                            if (numOfCoins <= 0)
                            {
                                validEntry = false;
                                Console.WriteLine("Number of coins must be greater than 0.");
                            }
                            break;
                        case 2: //target value
                            target = tempDouble;
                            if (target <= 0)
                            {
                                validEntry = false;
                                Console.WriteLine("Target must be greater than 0.");
                            }
                            break;
                        case 3: //population size
                            popSize = tempInt;
                            if (popSize <= 0)
                            {
                                validEntry = false;
                                Console.WriteLine("Population size must be greater than 0.");
                            }
                            break;
                        case 4: //number of children
                            numOfChildren = tempInt;
                            if (numOfChildren > popSize || numOfChildren <= 0)
                            {
                                validEntry = false;
                                Console.WriteLine("Number of children must be greater than 0 and less than or equal to the population size.");
                            }
                            break;
                        case 5: //tournament size
                            tournamentSize = tempInt;
                            if (tournamentSize <= 0 || tournamentSize > popSize)
                            {
                                validEntry = false;
                                Console.WriteLine("Tournament size must be greater than 0 and less than the population size.");
                            }
                            break;
                        case 6: //elitist 
                            eliteSize = ((tempInt * popSize) / 100);
                            if (eliteSize <= 0 || eliteSize > popSize)
                            {
                                validEntry = false;
                                Console.WriteLine("Elitist size must be greater than 0 and less than the population size.");
                            }
                            break;
                        case 7: //mutation rate
                            mutationRate = tempInt;
                            if (mutationRate < 0 || mutationRate > 100)
                            {
                                validEntry = false;
                                Console.WriteLine("Mutation Rate must be greater than or equal to 0 and less than or equal to 100.");
                            }
                            break;
                    } //end switch
                } //end if
            } //end while
        }

        /*
         * Function called to initialize all of the variables to valid states.
         * It will also generate a random starting string for each population 
         * index.  
         */
        void initialize()
        {
            Money coinList = new Money();
            for (int i = 0; i < popSize + numOfChildren; i++)
            {
                population.Add( coinList.generateRandomCoinList(numOfCoins) ); //create a new randomg coin list
                survivors.Add("");
                values.Add(0.00);
                fitness.Add(0.00);
            }
        }

        /*
         * This function is called to compute the values and the fitness of the 
         * population size.
         */
        void newPopValues()
        {
            Money coinList = new Money();
            for (int index = 0; index < popSize; index++)
            {
                values[index] = coinList.getValue(population[index]);
                fitness[index] = evaluateFitness(values[index]);
            }
        }

        /*
         * Small function used to calculate how close the fitness is to the target
         * value.  It will compute a value close to 0 if the value is close to the 
         * target, or a huge positive number if it is far away from the target.
         */
        double evaluateFitness(double value)
        {
            double temp = (target - value) / target;
            if (temp < 0.00)
                return (temp * -1);
            return temp;
        }

        /*
         * Function designed to compute the average value of the culled population.
         * It will also find the best fit value (closest to actual value), and 
         * outputs that as well.
         */
        void outputStats(int genCount)
        {
            double average = 0.00;
            int size = 0;
            double bestFitValue = 100.00;
            string bestFitList = "";
            for (int i = 0; i < (int)population.Count; i++)
            {
                average += values[i]; //increase average
                size++; //increase size
                if (fitness[i] < bestFitValue) //if the current value is better fit than the current best fit.
                {
                    bestFitValue = fitness[i];
                    bestFitList = population[i];
                }
            }
            Console.WriteLine("Generation: {0}  Avg Val: {1}  Best Fit: {2}", genCount + 1, Math.Round((double)(average / size), 2), bestFitList);
        }

        /*
         * sort(int, int) follows the Quick Sort algorithm.  It recursively calls
         * itself to quickly sort an unsorted list with lowest on left, highest
         * on right.
         */
        void sort(int left, int right)
        {
            int i = left, j = right;
            string tempList;
            double tempValue;
            double tempFitness;
            double pivot = fitness[(left + right) / 2]; //set the pivot value at halfway in the index

            //Partition the array, and find which elements need to be swapped
            while (i <= j)
            {
                while (fitness[i] < pivot) //increment i until we find a value that is greater than the pivot value
                    i++; 
                while (fitness[j] > pivot) //decrement j until we find a value that is less than the pivot value
                    j--;
                if (i <= j) //swap positions in the population, values, and fitness to keep everything matching together
                {
                    tempList = population[i];
                    tempValue = values[i];
                    tempFitness = fitness[i];
                    population[i] = population[j];
                    values[i] = values[j];
                    fitness[i] = fitness[j];
                    population[j] = tempList;
                    values[j] = tempValue;
                    fitness[j] = tempFitness;
                    i++;
                    j--;
                }
            };
            // recursively call function on smaller partitions to sort the array fully 
            if (left < j)
                sort(left, j);
            if (i < right)
                sort(i, right);
        }

        /*
         * Function designed to perform a similar function to the tournament selection
         * of genetic algorithms.  It gets the first element, then performs random picks
         * in the population for the tournament size.  
         */
        int tournamentSelection()
        {
            int index = rand.Next() % population.Count; //get the first random individual's index
            double currentLow = fitness[index]; //set the current low at the first pick
            int lowestIndex = index; //keep track of the best pick from the tournament.
            for (int i = 0; i < tournamentSize - 1; i++) //for the rest of the tournament size
            {
                index = rand.Next() % population.Count; //get a new index
                if (fitness[index] < currentLow) //if it's a new low, then set it as such
                {
                    currentLow = fitness[index];
                    lowestIndex = index;
                }
            }
            return lowestIndex; //return the lowest index for use later
        }

        /*
         * Fucntion designed to pick the first so many individuals specified by the user
         */
        void elitistSelection()
        {
            for (int i = 0; i < eliteSize; i++)
            {
                survivors[i] = population[i];
                survivorCount++;
            }
        }

        /*
         * "Main" function of the program.  It runs for the entire generation count, generating children, 
         * culling the population, and outputting the stats.  
         */
        public void run()
        {
            string parent1;
            string parent2;
            string child;
            Money coinList = new Money();

            for (int i = 0; i < numberOfVariables; i++) //Get the input
            {
                getInput(i);
            }
            initialize(); //Initialize the population
            for (int genCount = 0; genCount < generations; genCount++)
            {
                newPopValues(); //update the population values.
                survivorCount = 0;
                for (int childrenCount = 0; childrenCount < numOfChildren; childrenCount++)
                {
                    // Select 2 Random Parents, perform crossover
                    parent1 = population[rand.Next() % popSize];
                    parent2 = population[rand.Next() % popSize];
                    child = crossover(parent1, parent2); //random position, front half of a goes to child, back half of b goes to child
                    if ((rand.Next() % 100) < mutationRate) //percentage of chance for mutation
                        child = mutate(child); //pick random spot, replace value with new random value
                    // evaluate child
                    values[popSize + childrenCount] = coinList.getValue(child);
                    fitness[popSize + childrenCount] = evaluateFitness(values[popSize + childrenCount]);
                    // add child to population
                    population[popSize + childrenCount] = child;
                }
                //sort population by fitness
                sort(0, fitness.Count - 1);
                //elitism - copies certain number of top choices to survivors
                elitistSelection();
                //survivor selection - tournament (do until survivor is filled)
                while (survivorCount < popSize)
                {
                    survivors[survivorCount] = population[tournamentSelection()];
                    survivorCount++;
                }
                //population = survivors
                for (int i = 0; i < survivorCount; i++)
                {
                    population[i] = survivors[i];
                }
                //report stats - best individual, average fitness level
                outputStats(genCount);
            }
            //Memory Management
            population.Clear();
            values.Clear();
            survivors.Clear();
            fitness.Clear();
        }

        /*
         * Function to take a child, mutate a random position to have a new random value, then
         * return the mutated child.
         */
        string mutate(string toMutate)
        {
            int indexOfString = rand.Next() % (toMutate.Length);
            int newValue = rand.Next() % 6;
            string toReturnFromHere = "";
            for (int i = 0; i < toMutate.Length; i++) //Copy each element over to a new string, because testing showed that I could not manipulate a string at a given index
            {
                if (i == indexOfString) //if the current position is the one to be changed
                {
                    switch (newValue) //get a new value added on the end
                    {
                        case 0:
                            toReturnFromHere += 'p';
                            break;
                        case 1:
                            toReturnFromHere += 'n';
                            break;
                        case 2:
                            toReturnFromHere += 'd';
                            break;
                        case 3:
                            toReturnFromHere += 'q';
                            break;
                        case 4:
                            toReturnFromHere += 'h';
                            break;
                        case 5:
                            toReturnFromHere += 'o';
                            break;
                    }
                }
                else //otherwise, add the appropriate value over.
                    toReturnFromHere += toMutate[i];
            }
            return toReturnFromHere;
        }

        /*
         * Perform crossover on a random position between 2 parents to get the new child.
         */
        string crossover(string parent1, string parent2)
        {
            string newChild = "";
            int indexOfSwitch = rand.Next() % (int)parent1.Length; //pick a random index
            if (parent1.Length != parent2.Length)
                return "INVALID";
            for (int i = 0; i < indexOfSwitch; i++) //take from 0 to rhe index from the first parent
            {
                newChild += parent1[i]; //add characters from the parent
            }
            for (int i = indexOfSwitch; i < (int)parent1.Length; i++) //take from the index of the switch to the end from the second parent
            {
                newChild += parent2[i]; //add chatracters from the second parent
            }
            return newChild;
        }
    }
}
