namespace FuzzySim.Core
{
    using System;
    using System.Drawing;
    using CFLS;

    public static class Rule
    {
        /// <summary>
        /// FUZZY 'AND' RULE
        /// 
        /// "IF 
        ///     val1 is a member of fs1, 
        /// AND
        ///     val2 is a member of fs2
        /// THEN 
        ///     fs3 IS fs4
        /// </summary>
        /// <param name="val1">Antecedent 1</param> 
        /// <param name="fs1">Antecedent Set 1</param>
        /// <param name="val2">Antecedent 2</param>
        /// <param name="fs2">Antecedent Set 2</param>
        /// <param name="fs3">Consequent Output</param>
        /// <param name="fs4">Consequent</param>
        /// <param name="ftemp">Rule Applicability Output Set</param>
        public static FuzzySet AND(double val1, FuzzySet fs1, double val2, FuzzySet fs2, ref FuzzySet fs3, FuzzySet fs4,
                               FuzzySet ruleSet)
        {
            double lowRange = fs4.GetLowRange();
            double highRange = fs4.GetHighRange();


            //... and of the Consequent Output...
            string consName = fs3.Id;
            Brush consColour = fs3.LineColour;


            FuzzySet ftemp = new FuzzySet(ruleSet);
            ftemp.Clear();
            ftemp.SetRangeWithPoints(lowRange, highRange);
            
            double fval1 = fs1.Fuzzify(val1);
            double fval2 = fs2.Fuzzify(val2);

            double membership = Math.Min(fval1, fval2); //MIN for AND

            if (membership > 0)
            {
                FuzzySet oldRule = new FuzzySet(ftemp);

                ftemp = new FuzzySet(Operations.ScaleFS(fs4, membership));
                ftemp.LineColour = ruleSet.LineColour;
                ftemp.Id = ruleSet.Id;

                if (ftemp.GetNumPoints() < 2)
                    ftemp = oldRule;

                fs3 = new FuzzySet(Operations.UnionFS(fs3, ftemp, consColour));
                fs3.Id = consName;

            }

            return ftemp;
        }


        /// <summary>
        /// FUZZY 'OR' RULE
        /// 
        /// "IF 
        ///     val1 is a member of fs1, 
        /// OR
        ///     val2 is a member of fs2
        /// THEN 
        ///     fs3 IS fs4
        /// </summary>
        /// <param name="val1">Antecedent 1</param> 
        /// <param name="fs1">Antecedent Set 1</param>
        /// <param name="val2">Antecedent 2</param>
        /// <param name="fs2">Antecedent Set 2</param>
        /// <param name="fs3">Consequent Output</param>
        /// <param name="fs4">Consequent</param>
        /// <param name="ftemp">Rule Applicability Output Set</param>
        public static FuzzySet OR(double val1, FuzzySet fs1, double val2, FuzzySet fs2, ref FuzzySet fs3, ref FuzzySet fs4,
                               FuzzySet ruleSet)
        {
            double lowRange = fs4.GetLowRange();
            double highRange = fs4.GetHighRange();

            //... and of the Consequent Output...
            string consName = fs3.Id;
            Brush consColour = fs3.LineColour;

            FuzzySet ftemp = new FuzzySet(ruleSet);
            ftemp.Clear();
            ftemp.SetRangeWithPoints(lowRange, highRange);

            double fval1 = fs1.Fuzzify(val1);
            double fval2 = fs2.Fuzzify(val2);

            double membership = Math.Max(fval1, fval2); //MAX for OR

            if(membership > 0)
            {
                ftemp = Operations.ScaleFS(fs4, membership);
                ftemp.LineColour = ruleSet.LineColour;
                ftemp.Id = ruleSet.Id;

                fs3 = new FuzzySet(Operations.UnionFS(fs3, ftemp, consColour));
                fs3.Id = consName;
            }

            return ftemp;
        }


        /// <summary>
        /// FUZZY 'IS' RULE
        /// 
        /// "IF 
        ///     val1 is a member of fs1, 
        /// THEN 
        ///     fs3 IS fs4
        /// </summary>
        /// <param name="val1">Antecedent 1</param> 
        /// <param name="fs1">Antecedent Set 1</param>
        /// <param name="fs3">Consequent Output</param>
        /// <param name="fs4">Consequent</param>
        /// <param name="ftemp">Rule Applicability Output Set</param>
        public static FuzzySet IS(double val1, FuzzySet fs1, ref FuzzySet fs3, FuzzySet fs4,
                                FuzzySet ruleSet)
        {
            
            double lowRange = fs4.GetLowRange();
            double highRange = fs4.GetHighRange();

            string consName = fs3.Id;
            Brush consColour = fs3.LineColour;

            FuzzySet ftemp = new FuzzySet(ruleSet);
            ftemp.Clear();
            ftemp.SetRangeWithPoints(lowRange, highRange);


            double membership = fs1.Fuzzify(val1);

            if (membership > 0)
            {
                ftemp = Operations.ScaleFS(fs4, membership);
                ftemp.LineColour = ruleSet.LineColour;
                ftemp.Id = ruleSet.Id;


                fs3 = Operations.UnionFS(fs3, ftemp, ruleSet.LineColour);
                fs3.Id = consName;
                fs3.LineColour = consColour;
            }

            return ftemp;

        }
    }
}
