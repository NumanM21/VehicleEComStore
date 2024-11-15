namespace Core.Specifications;

public class ProductSpecParameters
{
    
    // Parameters to define the boundaries of our pagination and to track 
    private const int PageSizeMax = 25;
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 4;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > PageSizeMax) ? PageSizeMax : value;
    }
    
    
    private List<string> _brands = []; // Backing-field initialized to empty list 

    public List<string> Brands
    {
        get => _brands; // Will be brands = Aston Martin, Toyota (We want to return a list of brands without the ',' and separate strings
        set
        {
            // value is a keyword that will store the list of our items after we have done the .SelectMany (["car 1", "car 2"])
            _brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    
    private List<string> _fuelTypes= []; 

    public List<string> FuelTypes
    {
        get => _fuelTypes; 
        set
        {
            
            _fuelTypes = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    
    private List<string> _gearbox = []; 

    public List<string> Gearbox
    {
        get => _gearbox; 
        set
        {
            
            _gearbox = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    
    private List<string> _models = []; 

    public List<string> Models
    {
        get => _models; 
        set
        {
            
            _models = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { get; set; }
   
}