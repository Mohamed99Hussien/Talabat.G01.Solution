using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.IRepositories;
using Talabat.Core.IService;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Excensions
{
    public static class ApplicationServicesExtensions
    {
        
        public static IServiceCollection AddApplicationServices( this IServiceCollection services) 
        {
            services.AddTransient(typeof(ITokenService), typeof(TokenService));
            services.AddTransient(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddTransient(typeof(IOrderService), typeof(OrderService));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IPaymentService), typeof(PaymentService));
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));
            // validationError
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(M => M.Value.Errors.Count() > 0)
                                                        .SelectMany(M => M.Value.Errors)
                                                        .Select(E => E.ErrorMessage)
                                                        .ToArray();

                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };


            });

            return services;
        }
    }
}
