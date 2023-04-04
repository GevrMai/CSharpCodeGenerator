namespace CSharpCodeGenerator.Models
{
    public static class Check
    {
        public static bool CheckClassesNumber(Matrix Matrix, string classesNumber, bool fromFile)    // проверка количества классов и ввода числа
        {
            if (fromFile)
            {
                return true;
            }
            else
            {
                if (int.TryParse(classesNumber, out Matrix.numberOfClasses)
                                && Matrix.numberOfClasses <= 26 && Matrix.numberOfClasses > 0)
                {
                    Matrix.matrix = new int[Matrix.numberOfClasses, Matrix.numberOfClasses];
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
    }
}
