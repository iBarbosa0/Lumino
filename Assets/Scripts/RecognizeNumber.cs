using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Sentis;
using UnityEngine.UI;
using System.Linq;


public class RecognizeNumber : MonoBehaviour
{
    
     public ModelAsset mnistONNX;
    // engine type
    Worker engine;

    // This small model works just as fast on the CPU as well as the GPU:
    static BackendType backendType = BackendType.GPUCompute;

    // width and height of the image:
    const int imageWidth = 28;

    // input tensor
    Tensor<float> inputTensor = null;
    
    int predictedNumber;
    float probability;
    public Text predictionText, probabilityText;


    void Start()
    {
        // load the neural network model from the asset:
        Model model = ModelLoader.Load(mnistONNX);

        var graph = new FunctionalGraph();
        inputTensor = new Tensor<float>(new TensorShape(1, 1, imageWidth, imageWidth));
        var input = graph.AddInput(DataType.Float, new TensorShape(1, 1, imageWidth, imageWidth));
        var outputs = Functional.Forward(model, input);
        var result = outputs[0];

        // Convert the result to probabilities between 0..1 using the softmax function
        var probabilities = Functional.Softmax(result);
        var indexOfMaxProba = Functional.ArgMax(probabilities, -1, false);
        model = graph.Compile(probabilities, indexOfMaxProba);

        // create the neural network engine:
        engine = new Worker(model, backendType);
    }

    // Sends the image to the neural network model and returns the probability that the image is each particular digit.
    public (float, int) GetMostLikelyDigitProbability(Texture2D drawableTexture)
    {
        // Convert the texture into a tensor, it has width=W, height=W, and channels=1:
        TextureConverter.ToTensor(drawableTexture, inputTensor, new TextureTransform());

        // run the neural network:
        engine.Schedule(inputTensor);

        // We get a reference to the outputs of the neural network. Make the result from the GPU readable on the CPU
        using var probabilities = (engine.PeekOutput(0) as Tensor<float>).ReadbackAndClone();
        using var indexOfMaxProba = (engine.PeekOutput(1) as Tensor<int>).ReadbackAndClone();

        var predictedNumber = indexOfMaxProba[0];
        var probability = probabilities[predictedNumber];

        return (probability, predictedNumber);
    }
    
    public void Infer(Texture2D drawableTexture)
    {
        var probabilityAndIndex = GetMostLikelyDigitProbability(drawableTexture);

        probability = probabilityAndIndex.Item1;
        predictedNumber = probabilityAndIndex.Item2;
        Debug.Log("probability: " + probability * 100 + "%");
        Debug.Log("predicted number: " + predictedNumber);
        //predictionText.text = predictedNumber.ToString();
        if (probabilityText) probabilityText.text = Mathf.Floor(probability * 100) + "%";
    }
    
    private void OnDestroy()
    {
        inputTensor?.Dispose();
        engine?.Dispose();
    }
    
}
