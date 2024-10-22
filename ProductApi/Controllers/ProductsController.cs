using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;
using ProductApiAsync.Command;
using ProductApiAsync.Queries;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // private readonly IProductRepository _productRepository;
        /*public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository; // Dependency Injection

        }*/

        private readonly IProductService _productService;
        //private readonly IProductMapper _productMapper;

        //public ProductsController(IProductService productService,IProductMapper productMapper)
        //{
        //    _productService = productService; // Dependency Injection
        //    _productMapper = productMapper;

        //}
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator, IProductService productService)
        {
            _mediator = mediator; // Dependency Injection
            _productService = productService;
        }
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            // var products = await _productService.GetAllProductsAsync();
            //return Ok(products);
            return await _mediator.Send(new GetAllProductQuery());
        }

        [HttpGet("{id}")]
        public async Task<Product> Get(int id)
        {
            //var product = _productRepository.GetProductById(id);
            //var product = await _productService.GetProductByIdAsync(id);
            //if (product == null)
            //{
            //    return NotFound();
            //}
            //var productDto = _productMapper.MapToDto(product);

            //return Ok(productDto);

            return await _mediator.Send(new GetProductByIdQuery { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromForm] Product product, IFormFile productImage)
        {
            var createdProduct = await _mediator.Send(new CreateProductCommand(
                product.Name, product.Description, product.Price, product.ImageUrl, productImage));

            // Generate the image URL
            var imageUrl = Url.Action("GetImages", "Products", new { fileName = createdProduct.ImageUrl }, Request.Scheme);

            // Return product details along with the image URL
            return Ok(new
            {
                createdProduct.Id,
                createdProduct.Name,
                createdProduct.Description,
                createdProduct.Price,
                ImageUrl = imageUrl
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] Product product, IFormFile productImage)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var productReturn = await _mediator.Send(new UpdateProductCommand(
                id,
                product.Name,
                product.Description,
                product.Price,
                product.ImageUrl,
                productImage
            ));

            return Ok(productReturn);
        }

        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            //await _productService.DeleteProductAsync(id);
            //return NoContent();
            return await _mediator.Send(new DeleteProductCommand { Id = id });
        }
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile productImage, int productId)
        {
            if (productImage == null || productImage.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            try
            {
                // Generate a unique file name using GUID
                var fileName = Guid.NewGuid() + Path.GetExtension(productImage.FileName);
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                // Check if the directory exists, if not, create it
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Combine the directory and file name to get the full file path
                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productImage.CopyToAsync(stream);
                }

                // Save product info to the database
                var imageDetail = new ImageDetail
                {
                    ProductImage = fileName,
                    ImagePath = filePath
                };

                await _productService.UploadFileAsync(imageDetail);

                // Update the product with the image name
                var product = await _productService.GetProductByIdAsync(productId);
                if (product != null)
                {
                    product.ImageUrl = fileName;
                    await _productService.UpdateProductAsync(product);
                }

                return Ok(new { message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }


        /* [HttpPost("UploadBase64Image")]
         public async Task<IActionResult> UploadBase64Image([FromBody] ImageDetail model)
         {
             if (string.IsNullOrEmpty(model.Base64Image))
             {
                 return BadRequest(new { message = "Invalid image data" });
             }

             try
             {
                 var imageBytes = Convert.FromBase64String(model.Base64Image);
                 var fileName = Guid.NewGuid() + ".png";
                 var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                 System.IO.File.WriteAllBytes(filePath, imageBytes);

                 //save product info to the dataabase
                 var imageDetail = new ImageDetail
                 {
                     ProductImage = fileName,
                     ImagePath = filePath
                 };
                 await _productService.UploadFileAsync(imageDetail);
                 return Ok(new { message = "File upload successfully" });
             }
             catch (Exception ex)
             {
                 return StatusCode(500, new { message = "Error saving image", error = ex.Message });
             }
         }
 */
        

        //support multiple image extensions

 /*       [HttpGet("GetImages")]
        public async Task<IActionResult> GetImages(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is not provided.");
            }

            try
            {
                // List of supported image extensions
                var supportedExtensions = new[] { ".png", ".jpeg", ".jpg" };
                string filePath = null;
                string fileExtension = null;

                // Check for each supported extension
                foreach (var extension in supportedExtensions)
                {
                    var potentialFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName + extension);
                    if (System.IO.File.Exists(potentialFilePath))
                    {
                        filePath = potentialFilePath;
                        fileExtension = extension;
                        break;
                    }
                }

                if (filePath == null)
                {
                    return NotFound("File not found.");
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var mimeType = fileExtension switch
                {
                    ".png" => "image/png",
                    ".jpeg" => "image/jpeg",
                    ".jpg" => "image/jpeg",
                    _ => "application/octet-stream"
                };

                return File(fileBytes, mimeType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
*/

        [HttpGet("GetImages")]
        public async Task<IActionResult> GetImages(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is not provided.");
            }
            try
            {
                //support single image extension
                string fileExtension = ".jpeg";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName + fileExtension);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File not found.");

                }
                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(fileBytes, "image/jpeg"); // Adjust MIME type based on image format

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            
            }



        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is not provided.");
            }
            string fileExtension = ".jpeg";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName + fileExtension);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            //Generate the image URL asynchronously
            var imageUrl = await Task.Run(() => Url.Action("GetImages", "ProductsApi", new { fileName = fileName }, Request.Scheme));

            // Return JSON with the file name and image URL
            return Ok(new { fileName = fileName, imageUrl = imageUrl });

            

        }
     



        // Action to get Base64 encoded image
        /* [HttpGet("GetBase64Image")]
         public async Task<IActionResult> GetBase64Image(int id)
         {
             //var product = _productRepository.GetProductById(id);
             //if (product == null || string.IsNullOrEmpty(product.ProductImage))
             //{
             //    return NotFound("Product or image not found.");
             //}
             string fileName = "d489e21c-8118-4d82-82b6-8e8049797f76";
             string fileExtension = ".png";
             var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName + fileExtension);
             if (!System.IO.File.Exists(filePath))
             {
                 return NotFound("Image file not found.");
             }

             var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
             var base64String = Convert.ToBase64String(fileBytes);

             // Use Ok() to return the JSON response
             return Ok(new { base64Image = "data:image/png;base64," + base64String });
         }
 */

    }
}
