namespace NeuralNetwork.ServicesManager.Vectors
{
    public struct Coeficent
    {
        public string _word;
        public double[] _listFloat;

        public Coeficent(string word, double[] listFloat)
        {
            _word = word;
            _listFloat = listFloat;
        }
    }
}
