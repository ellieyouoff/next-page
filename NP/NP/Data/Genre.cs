using System;
using System.ComponentModel.DataAnnotations;

namespace NP.Models
{
    public enum Genre
    {
        Comedy = 1,
        Horror,
        Fiction,
        [Display(Name = "Science Fiction")]
        ScienceFiction,
        Fantasy,
        Dystopian,
        [Display(Name = "Action & Adventure")]
        ActionAdventure,
        Mystery,
        [Display(Name = "Thriller & Suspense")]
        ThrillerSuspense,
        [Display(Name = "Historical Fiction")]
        HistoricalFiction,
        Romance,
        [Display(Name = "Women's Fiction")]
        WomensFiction,
        [Display(Name = "LGBTQ+")]
        LGBTQ,
        [Display(Name = "Contemporary Fiction")]
        ContemporaryFiction,
        [Display(Name = "Literary Fiction")]
        LiteraryFiction,
        [Display(Name = "Magical Realism")]
        MagicalRealism,
        [Display(Name = "Graphic Novel")]
        GraphicNovel,
        [Display(Name = "Young Adult")]
        YoungAdult,
        [Display(Name = "Children's")]
        Childrens,
        Biography,
        Autobiography,
        Hobbies,
        [Display(Name = "Self-Help")]
        SelfHelp,
        [Display(Name = "True Crime")]
        TrueCrime,
        [Display(Name = "Non-Fiction")]
        NonFiction,



    }
}

