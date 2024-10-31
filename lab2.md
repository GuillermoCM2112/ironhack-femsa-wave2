## CODE EXAMPLE
```
class SystemManager {
    processOrder(order) {
        if (order.type == "standard") {
            verifyInventory(order);
            processStandardPayment(order);
        } else if (order.type == "express") {
            verifyInventory(order);
            processExpressPayment(order, "highPriority");
        }
        updateOrderStatus(order, "processed");
        notifyCustomer(order);
    }

    verifyInventory(order) {
        // Checks inventory levels
        if (inventory < order.quantity) {
            throw new Error("Out of stock");
        }
    }

    processStandardPayment(order) {
        // Handles standard payment processing
        if (paymentService.process(order.amount)) {
            return true;
        } else {
            throw new Error("Payment failed");
        }
    }

    processExpressPayment(order, priority) {
        // Handles express payment processing
        if (expressPaymentService.process(order.amount, priority)) {
            return true;
        } else {
            throw new Error("Express payment failed");
        }
    }

    updateOrderStatus(order, status) {
        // Updates the order status in the database
        database.updateOrderStatus(order.id, status);
    }

    notifyCustomer(order) {
        // Sends an email notification to the customer
        emailService.sendEmail(order.customerEmail, "Your order has been processed.");
    }
}
```

## TASK 1 REVISION CÓDIGO
Al revisar el código, si viola los principios de solid. En la Task 3 se documentara cuales son

## TASK 2 REFACTOR CÓDIGO

```
public interface IPaymentService{
    Process(amount)
}

public interface IOrderService{
    UpdateStatus(idOrder, status)
}

public interface IProductService{
    VerifyInventory(quantity)
}

public interface INotificationService{
    Send(customerEmail)
}

class PaymentStandardService : IPaymentService {
    process(amount) {
        if (paymentService.process(amount)) {
            return true;
        } else {
            throw new Error("Payment failed");
        }
    }
}

class PaymentExpressService : IPaymentService {
    process(amount) {
        if (expressPaymentService.process(amount, "highPriority")) {
            return true;
        } else {
            throw new Error("Express payment failed");
        }
    }
}

class PaymentFactory {
    public static IPaymentService GetPaymentService(type) {
        switch (type) {
            case "standard":
                return new PaymentStandardService(new PaymentService());
            case "express":
                return new PaymentExpressService(new ExpressPaymentService());
            default:
                throw new Error("Invalid order type");
        }
    }
}

public class OrderService : IOrderService
{
    UpdateStatus(int idOrder, string status)
    {
        database.updateOrderStatus(idOrder, status);
    }
}

public class ProductService : IProductService
{
    VerifyInventory(quantity) {
        if (inventory < quantity) {
            throw new Error("Out of stock");
        }
    }
}

public class NotificationService : INotificationService
{
    Send(string customerEmail)
    {
        emailService.sendEmail(customerEmail, "Your order has been processed.");
    }
}

class OrderManager {
    private readonly IProductService productService;
    private readonly IOrderService orderService;
    private readonly IPaymentService paymentService;
    private readonly INotificationService notificationService;

    public OrderManager (productService, orderService, notificationService) {
        this.productService = productService;
        this.orderService = orderService;
        this.notificationService = notificationService;
    }

    public void processOrder(order) {
        this.productService.verifyInventory(order.quantity);

        IPaymentService paymentService = PaymentFactory.GetPaymentService(order.type);
        paymentService.Process(order.amount);

        this.orderService.UpdateStatus(order.id, "processed");
        this.notificationService.Send(order.customerEmail);
    }
}
```

## TASK 3 EXPLICACION

#### CODIGO ANTERIOR
```
class SystemManager {
    processOrder(order) {
        if (order.type == "standard") {
            verifyInventory(order);
            processStandardPayment(order);
        } else if (order.type == "express") {
            verifyInventory(order);
            processExpressPayment(order, "highPriority");
        }
        updateOrderStatus(order, "processed");
        notifyCustomer(order);
    }

    verifyInventory(order) {
        // Checks inventory levels
        if (inventory < order.quantity) {
            throw new Error("Out of stock");
        }
    }

    processStandardPayment(order) {
        // Handles standard payment processing
        if (paymentService.process(order.amount)) {
            return true;
        } else {
            throw new Error("Payment failed");
        }
    }

    processExpressPayment(order, priority) {
        // Handles express payment processing
        if (expressPaymentService.process(order.amount, priority)) {
            return true;
        } else {
            throw new Error("Express payment failed");
        }
    }

    updateOrderStatus(order, status) {
        // Updates the order status in the database
        database.updateOrderStatus(order.id, status);
    }

    notifyCustomer(order) {
        // Sends an email notification to the customer
        emailService.sendEmail(order.customerEmail, "Your order has been processed.");
    }
}
```
**Comentarios sobre los Principios SOLID:**
**SRP (Single Responsibility Principle):** La clase SystemManager tiene múltiples responsabilidades (gestionar inventarios, procesar pagos, actualizar estados y notificar clientes), lo que la hace difícil de mantener y extender. Cada responsabilidad debe ser manejada por clases específicas.

**OCP (Open/Closed Principle):** La clase SystemManager no está abierta para la extensión. Si se desea añadir un nuevo tipo de orden o una nueva forma de procesar pagos, el método processOrder debe ser modificado, lo cual no es deseable.

**LSP (Liskov Substitution Principle):** La clase SystemManager no utiliza interfaces para los servicios de pago, lo que significa que no se pueden sustituir fácilmente las implementaciones sin modificar la clase.

**ISP (Interface Segregation Principle):** No se aplican interfaces en la clase, lo que significa que la clase no está separada en métodos específicos y no se puede dividir en interfaces más pequeñas.

**DIP (Dependency Inversion Principle):** SystemManager depende de implementaciones concretas (como paymentService y emailService) en lugar de interfaces o abstracciones, lo que dificulta las pruebas y la reutilización del código.


#### CÓDIGO NUEVO
```
public interface IPaymentService
{
    bool Process(decimal amount); // Método para procesar el pago.
}

public interface IOrderService
{
    void UpdateStatus(int idOrder, string status); // Método para actualizar el estado de la orden.
}

public interface IProductService
{
    void VerifyInventory(int quantity); // Método para verificar el inventario.
}

public interface INotificationService
{
    void Send(string customerEmail); // Método para enviar notificaciones.
}

public class PaymentStandardService : IPaymentService
{
    public bool Process(decimal amount)
    {
        // Aquí se debería utilizar una instancia de PaymentService inyectada.
        if (PaymentService.Process(amount)) // Se asume que PaymentService es una clase concreta.
        {
            return true;
        }
        else
        {
            throw new Exception("Payment failed");
        }
    }
}

public class PaymentExpressService : IPaymentService
{
    public bool Process(decimal amount)
    {
        // Aquí se debería utilizar una instancia de ExpressPaymentService inyectada.
        if (ExpressPaymentService.Process(amount, "highPriority")) // Se asume que ExpressPaymentService es una clase concreta.
        {
            return true;
        }
        else
        {
            throw new Exception("Express payment failed");
        }
    }
}

public static class PaymentFactory
{
    public static IPaymentService GetPaymentService(string type)
    {
        // Aplicación del OCP (Open/Closed Principle): este método puede ser extendido sin modificarlo.
        switch (type)
        {
            case "standard":
                return new PaymentStandardService(); // Retorna un servicio de pago estándar.
            case "express":
                return new PaymentExpressService(); // Retorna un servicio de pago express.
            default:
                throw new ArgumentException("Invalid order type");
        }
    }
}

public class OrderService : IOrderService
{
    public void UpdateStatus(int idOrder, string status)
    {
        // Método que actualiza el estado de la orden en la base de datos.
        Database.UpdateOrderStatus(idOrder, status); // Asume que Database es una clase estática que gestiona las operaciones con la base de datos.
    }
}

public class ProductService : IProductService
{
    public void VerifyInventory(int quantity)
    {
        // Este método verifica si hay suficiente inventario.
        if (Inventory < quantity) // Se asume que Inventory es una variable que almacena la cantidad de productos disponibles.
        {
            throw new Exception("Out of stock");
        }
    }
}

public class NotificationService : INotificationService
{
    public void Send(string customerEmail)
    {
        // Método que envía un correo electrónico al cliente.
        EmailService.SendEmail(customerEmail, "Your order has been processed."); // Asume que EmailService es una clase que gestiona el envío de correos.
    }
}

public class OrderManager
{
    private readonly IProductService productService; // Inyección de dependencia.
    private readonly IOrderService orderService; // Inyección de dependencia.
    private readonly INotificationService notificationService; // Inyección de dependencia.

    // Constructor de OrderManager que recibe las interfaces necesarias.
    public OrderManager(IProductService productService, IOrderService orderService, INotificationService notificationService)
    {
        this.productService = productService;
        this.orderService = orderService;
        this.notificationService = notificationService;
    }

    public void ProcessOrder(Order order) // Se asume que Order es una clase que contiene información sobre la orden.
    {
        this.productService.VerifyInventory(order.Quantity); // Verifica la disponibilidad del producto.

        IPaymentService paymentService = PaymentFactory.GetPaymentService(order.Type); // Obtiene el servicio de pago adecuado.
        paymentService.Process(order.Amount); // Procesa el pago.

        this.orderService.UpdateStatus(order.Id, "processed"); // Actualiza el estado de la orden.
        this.notificationService.Send(order.CustomerEmail); // Envía una notificación al cliente.
    }
}
```
**Comentarios sobre la Aplicación de SOLID:**
**SRP (Single Responsibility Principle):**
Cada clase tiene una única responsabilidad. Por ejemplo, OrderService solo actualiza el estado de la orden y NotificationService solo envía correos electrónicos.

**OCP (Open/Closed Principle):**
El método GetPaymentService en PaymentFactory permite la extensión del sistema para incluir nuevos tipos de pagos sin modificar el código existente. Esto significa que si necesitas agregar un nuevo tipo de servicio de pago, puedes hacerlo sin cambiar la lógica de la fábrica.

**LSP (Liskov Substitution Principle):**
Cualquier implementación de IPaymentService puede ser utilizada en lugar de la clase base sin alterar la funcionalidad. Por ejemplo, tanto PaymentStandardService como PaymentExpressService pueden ser utilizados de manera intercambiable en el OrderManager.

**ISP (Interface Segregation Principle):**
Cada interfaz está diseñada para tener métodos específicos, evitando que las implementaciones dependan de métodos que no utilizan. Por ejemplo, INotificationService solo tiene un método Send, que es específico para enviar notificaciones.

**DIP (Dependency Inversion Principle):**
OrderManager depende de interfaces (abstracciones) en lugar de clases concretas, facilitando la inyección de dependencias y permitiendo que el sistema se adapte fácilmente a cambios en la implementación.
