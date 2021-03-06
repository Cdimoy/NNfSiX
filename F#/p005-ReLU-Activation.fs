open NumSharp

let spiral_data points classes =
    let X = np.zeros [|(points * classes); 2|]
    let y = np.zeros (points * classes)

    for classNumber in [0..classes-1] do
        let r = np.linspace(0.0, 1.0, points)
        let t = (np.linspace(float (classNumber*4), float ((classNumber + 1) * 4), points) + (np.random.randn points) * 0.2) * 2.5

        let sv = r * np.sin(&t)
        let cv = r * np.cos(&t)

        // NumSharp doesn't have a c_ or r_ yet
        for j, i in List.indexed [points*classNumber..points*(classNumber + 1) - 1] do
            X.[$"{i}, 0"] <- sv.[j]
            X.[$"{i}, 1"] <- cv.[j]
            y.SetDouble(float classNumber, i)
            
    X, y

let X, y = spiral_data 100 3

type LayerDense(nInputs : int, nNeurons : int) = 
    let weights = 0.1 * np.random.randn [|nInputs; nNeurons|]
    let biases  = np.zeros [|1; nNeurons|]

    member _.Forward inputs = 
        np.dot(&inputs, &weights) + biases

type ActivationReLU() =
    static member Forward array = 
        let zero = np.zeros_like array
        np.maximum(&zero, &array)

let layer1 = LayerDense(2, 5)

System.Console.WriteLine(ActivationReLU.Forward << layer1.Forward <| X)
