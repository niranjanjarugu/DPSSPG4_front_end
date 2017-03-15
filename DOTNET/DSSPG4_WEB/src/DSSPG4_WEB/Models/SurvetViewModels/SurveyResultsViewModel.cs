using DSSPG4_WEB.Models.Entities;
using DSSPG4_WEB.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.SurvetViewModels
{
    public class SetCluster
    {
        public int surveyID { get; set; }
        [Display(Name = "Number of Clusters")]
        public int clusterCount { get; set; }
    }


    public class SurveyResultsViewModel
    {
        public double ResponsesMarkedImportant { get; set; }
        public double ResponsesMarkedNotImportant { get; set; }
        public Survey Survey { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
    }

    public class SurveyResultsByUserViewModel
    {

        public User User { get; set; }
        public Survey Survey { get; set; }
        public int Cluster { get; set; }
        public List<SurveyViewModelResponseData> Responses { get; set; }

        public void setCluster(int cluster)
        {
            this.Cluster = cluster;
        }

        public int getCluster()
        {
            return this.Cluster;
        }
    }

    public class SurveyViewModelResponseData
    {
        public string Question { get; set; }
        public ResponseValues ResponseValue { get; set; }
    }

    public class SurveyViewModelResponseDataCSV
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Question { get; set; }
        public ResponseValues ResponseValue { get; set; }
    }
}
