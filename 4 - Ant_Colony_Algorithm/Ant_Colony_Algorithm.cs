
class AntColonyAlgorithm
{

        class Path
        {
            public ArrayList Cities { get; set; }
            public List<int> CitiesIndexes { get; set; }
            public double Cost { get; set; }

            public Path()
            {
                Cities = new ArrayList();
                CitiesIndexes = new List<int>();
                Cost = 0;
            }
        }


        private const int SECONDS_LIMIT = 60;
        private const int ANTS_RELEASED = 50;

        private const double PHEROMONE_DEPOSIT_RATE = 1.001;
        private const double PHEROMONE_EVAPORATION_RATE = 0.9993;

        private const double ALPHA_WILLING_TO_FOLLOW_PHEROMENES = 0.005;        // Choose between 0 and 1 (less is better because it keeps exploring)
        private const double BETA_WILLING_TO_CHOOSE_LOCAL_SHORTEST_PATH = 0.4;  // Choose between 0 and 1 (more is better because it grabs less crazy paths)
        private const double RANDOM_WILLING_TO_EXPLORE = 0.25;                  // Choose between 0 and 1 (less is better because it jumps less)

        private List<bool> cityVisited;
        private List<List<double>> pheromones;
        private List<List<double>> distances;

        private Random random = new Random();
        private Stopwatch stopwatch = new Stopwatch();



        public void solveProblem()
        {
            stopwatch.Start();
            Path globalBestPath = null;
            int counter = 0;
            try
            {
                pheromones = InitializePheromones();
                distances = InitializeDistances();
                while (true)
                {
                    int startingCityIndex = counter % Cities.Length;
                    List<Path> listOfPaths = ReleaseAnts(ANTS_RELEASED, startingCityIndex);
                    Path localBestPath = GrabLocalBestPathFromTheseAnts(listOfPaths);
                    globalBestPath = CompareLocalBestPathToGlobalBestPath(localBestPath, globalBestPath);
                    PheromonesAddedToBestPath(localBestPath);
                    PheromonesAddedToBestPath(globalBestPath);
                    PheromonesEvaporating();
                    counter++;
                    /*
                    bssf = PopulateBssf(localBestPath);
                    Program.MainForm.tbCostOfTour.Text = " " + globalBestPath.Cost + " count " + counter;
                    Program.MainForm.Refresh();
                    */
                }
            }
            catch (Exception)
            {
                bssf = PopulateBssf(globalBestPath);
                Program.MainForm.tbCostOfTour.Text = " " + bssf.costOfRoute() + " count " + counter;
                Program.MainForm.tbElapsedTime.Text = " " + stopwatch.Elapsed;
                Program.MainForm.Invalidate();
                Program.MainForm.Refresh();
                System.Windows.Forms.Clipboard.SetText(bssf.ToString());
                stopwatch.Reset();
            }
        }




        private List<List<double>> InitializeDistances()
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                List<List<double>> outside = new List<List<double>>();
                for (int a = 0; a < Cities.Length; a++)
                {
                    List<double> inside = new List<double>();
                    for (int b = 0; b < Cities.Length; b++)
                    {
                        if (a == b)
                        {
                            inside.Add(0);
                        }
                        else
                        {
                            inside.Add(Cities[a].costToGetTo(Cities[b]));
                        }
                    }
                    outside.Add(inside);
                }
                return outside;
            }
            stopwatch.Stop();
            throw new Exception("InitializePheromones");
        }




        private List<List<double>> InitializePheromones()
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                double PHEROMONE_STARTING = 7.0;
                List<List<double>> outside = new List<List<double>>();
                for (int a = 0; a < Cities.Length; a++)
                {
                    List<double> inside = new List<double>();
                    for (int b = 0; b < Cities.Length; b++)
                    {
                        if (a == b)
                        {
                            inside.Add(0);
                        }
                        else
                        {
                            inside.Add(PHEROMONE_STARTING);
                        }
                    }
                    outside.Add(inside);
                }
                return outside;
            }
            stopwatch.Stop();
            throw new Exception("InitializePheromones");
        }




        private void PheromonesEvaporating()
        {
            foreach (List<double> listPheromones in pheromones)
            {
                for (int index = 0; index < listPheromones.Count; index++)
                {
                    listPheromones[index] *= PHEROMONE_EVAPORATION_RATE;
                }
            }
        }




        private Path CompareLocalBestPathToGlobalBestPath(Path LocalBestPath, Path globalBestPath)
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                if (globalBestPath == null)
                {
                    return LocalBestPath;
                }
                else
                {
                    if (LocalBestPath.Cost < globalBestPath.Cost)
                    {
                        return LocalBestPath;
                    }
                    else
                    {
                        return globalBestPath;
                    }
                }

            }
            stopwatch.Stop();
            throw new Exception("CompareBestPathToBSSF");
        }




        private TSPSolution PopulateBssf(Path globalBestPath)
        {
            ArrayList cities = new ArrayList();
            foreach (int cityIndex in globalBestPath.CitiesIndexes)
            {
                cities.Add(Cities[cityIndex]);
            }
            return new TSPSolution(cities);
        }




        private void PheromonesAddedToBestPath(Path path)
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                for (int index = 0; index < path.CitiesIndexes.Count - 1; index++)
                {
                    int currentcityIndex = path.CitiesIndexes[index];
                    int nextCityIndex = path.CitiesIndexes[index + 1];
                    pheromones[currentcityIndex][nextCityIndex] *= PHEROMONE_DEPOSIT_RATE;
                }
                return;
            }
            stopwatch.Stop();
            throw new Exception("AddPheromonesToBSSFPath");
        }




        private Path GrabLocalBestPathFromTheseAnts(List<Path> listOfPaths)
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                Path bestPath = new Path();
                bestPath.Cost = double.MaxValue;
                foreach (Path path in listOfPaths)
                {
                    if (path.Cost < bestPath.Cost)
                    {
                        bestPath = path;
                    }
                }
                return bestPath;
            }
            stopwatch.Stop();
            throw new Exception("GrabBestPathTheAntsTraveled");
        }




        private List<Path> ReleaseAnts(int ants, int startingCityIndex)
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                List<Path> paths = new List<Path>();
                for (int ant = 0; ant < ants; ant++)
                {
                    Path path = AntGoingOnATrip(startingCityIndex);
                    paths.Add(path);
                }
                return paths;
            }
            stopwatch.Stop();
            throw new Exception("ReleaseAnts");
        }




        private Path AntGoingOnATrip(int startingCityIndex)
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                cityVisited = new List<bool>(new bool[Cities.Length]);
                Path path = new Path();

                path.CitiesIndexes.Add(startingCityIndex);

                int nextCityIndex = 0;
                for (int currentCityIndex = startingCityIndex; path.CitiesIndexes.Count <= Cities.Length; currentCityIndex = nextCityIndex)
                {
                    nextCityIndex = AntWalksToNextCity(startingCityIndex, currentCityIndex);

                    path.CitiesIndexes.Add(nextCityIndex);
                    path.Cost += distances[currentCityIndex][nextCityIndex];

                    if (path.CitiesIndexes.Count == Cities.Length)
                    {
                        path.CitiesIndexes.Add(startingCityIndex);
                        path.Cost += distances[nextCityIndex][startingCityIndex];
                    }
                }
                return path;
            }
            stopwatch.Stop();
            throw new Exception("AntGoingOnATrip");
        }




        private int AntWalksToNextCity(int startingCityIndex, int currentCityIndex)
        {
            if (stopwatch.Elapsed.TotalSeconds < SECONDS_LIMIT)
            {
                double greatestProbabilityToBeChosen = double.MinValue;
                int cityWithBestProbabilityIndex = 0;
                for (int nextCityIndex = 0; nextCityIndex < Cities.Length; nextCityIndex++)
                {
                    if (cityVisited[nextCityIndex] == false && nextCityIndex != currentCityIndex && nextCityIndex != startingCityIndex)
                    {
                        double distance = distances[currentCityIndex][nextCityIndex] * BETA_WILLING_TO_CHOOSE_LOCAL_SHORTEST_PATH + 0.1;
                        double pheromone = pheromones[currentCityIndex][nextCityIndex] * ALPHA_WILLING_TO_FOLLOW_PHEROMENES + 0.1;
                        double exploring = random.NextDouble() * RANDOM_WILLING_TO_EXPLORE + 0.1;
                        double currentProbability = ( pheromone / distance ) * exploring;
                        if (currentProbability > greatestProbabilityToBeChosen)
                        {
                            greatestProbabilityToBeChosen = currentProbability;
                            cityWithBestProbabilityIndex = nextCityIndex;
                        }
                    }
                }

                cityVisited[cityWithBestProbabilityIndex] = true;
                return cityWithBestProbabilityIndex;
            }
            stopwatch.Stop();
            throw new Exception("AntWalksToNextCity");
        }

}
