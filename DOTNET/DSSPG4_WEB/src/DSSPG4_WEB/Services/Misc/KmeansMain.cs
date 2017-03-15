using DSSPG4_WEB.Models.SurvetViewModels;
using System;
using System.Collections.Generic;

namespace DSSPG4_WEB.Services.Misc
{
    public class KmeansMain
    {
        public KmeansMain(List<SurveyResultsByUserViewModel> data, int numCluster, int numQuestions)
        {
            this.dataSet = data;
            this.numClusters = numCluster;
            this.numOfQs = numQuestions;
            this.numOfUsers = dataSet.Count;
            this.updateCount = 0;
        }

        public KmeansMain(int numClusters, int numQuestions)
        {
            this.dataSet = new List<SurveyResultsByUserViewModel>();
            this.numClusters = numClusters;
            this.numOfQs = numQuestions;
            this.updateCount = 0;
        }

        public List<SurveyResultsByUserViewModel> dataSet { get; set; }
        public int numClusters { get; set; }
        public int numOfQs { get; set; }
        public int numOfUsers { get; set; }

        public int updateCount { get; set; }

        public IList<SurveyResultsByUserViewModel> Cluster()
        {
            bool changed = true;
            bool success = true;

            double[][] means = InitMeans(0);

            int maxCount = numOfUsers * 10; //limiting the amount of loops
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                ++ct;
                success = UpdateMeans(means);
                changed = UpdateClusterMembership(means);
            }

            return dataSet;
        }

        private double[][] InitMeans(int seed)
        {//initializes centroids to random value
            Random random = new Random(seed);
            double[][] means = Allocate();
            for (int c = 0; c < numClusters; ++c)
            {
                for (int q = 0; q < numOfQs; ++q)
                {
                    means[c][q] = random.Next(-2, 2);
                }
            }
            return means;
        }

        private bool UpdateMeans(double[][] means) // working on
        {
            int numOfMeans = means.Length;
            double[] newMean = new double[numOfQs];
            int count;

            for (int i = 0; i < numOfMeans; ++i)//do we need to ++ before or after?
            {
                count = 0;
                for (int j = 0; j < numOfUsers; ++j)//same here
                {

                    if (dataSet[j].Cluster == i)
                    {
                        count += 1;

                        for (int q = 0; q < numOfQs; ++q)//same here
                        {
                            newMean[q] += (double)dataSet[j].Responses[q].ResponseValue;
                        }

                    }
                    for (int q = 0; q < numOfQs; ++q)
                    {
                        newMean[q] /= count;
                    }
                }

            }
            return true;
        }

        private bool UpdateClusterMembership(double[][] means)//should work now
        {
            bool changed = false;
            int numOfMeans = means.Length;

            for (int j = 0; j < numOfUsers; ++j)//do we need to ++ before or after?
            {
                double[] dists = new double[numOfMeans];
                for (int i = 0; i < numOfMeans; ++i)//same here
                {
                    dists[i] = EuclideanDistance(j, means[i]);
                }
                int min = MinIndex(dists); //returns the index of minimum value

                if (dataSet[j].Cluster != min )
                {
                    dataSet[j].Cluster = min;
                    this.updateCount += 1;
                    changed = true;
                }
            }
            return changed;

        }

        private double EuclideanDistance(int userIndex, double[] meanInput) //should be fine now
        {
            double sumSquaredDiffs = 0.0;

            for (int j = 0; j < numOfQs; ++j)
                sumSquaredDiffs += Math.Pow(((int)dataSet[userIndex].Responses[j].ResponseValue - (int)meanInput[j]), 2);
            return Math.Sqrt(sumSquaredDiffs);
        }

        private int MinIndex(double[] distances)
        {
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }

        public double[][] Allocate() //works now 
        {
            double[][] result = new double[numClusters][];
            for (int k = 0; k < numClusters; ++k)
                result[k] = new double[numOfQs];
            return result;
        }

        public int getUpdateCount()
        {
            return this.updateCount;
        }

        public int getDataSetCount()
        {
            return this.dataSet.Count;
        }

        public int getUserCount()
        {
            return this.numOfUsers;
        }

        public int getResponseCount()
        {
            int responses = 0;
            foreach(var data in dataSet)
            {
                foreach(var res in data.Responses)
                {
                    responses += 1;
                }
            }
            return responses;
        }
    }
}
