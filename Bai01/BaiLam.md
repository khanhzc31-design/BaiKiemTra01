# phần lý thuyết và câu hỏi ngắn: 
---

## Câu 1: Trình bày sự khác nhau giữa Value Types (Kiểu giá trị) và Reference Types (Kiểu tham chiếu) trong C# về cơ chế lưu trữ vùng nhớ (Stack vs Heap)

### 1. Cơ chế lưu trữ và quản lý bộ nhớ
- Value Types (Kiểu giá trị): Lưu trữ trực tiếp dữ liệu thực tế tại ô nhớ được cấp phát.
  + Khi là biến cục bộ được khai báo bên trong phương thức, dữ liệu sẽ được lưu trữ trực tiếp trên vùng nhớ Stack.
  + Khi là một thuộc tính hoặc trường nằm bên trong một Reference Type (ví dụ một biến kiểu int nằm trong một Class), giá trị của nó sẽ được lưu trên Heap cùng với đối tượng chứa nó.
  + Vùng nhớ Stack được quản lý theo cơ chế LIFO (Last In, First Out) và tự động giải phóng ngay khi biến ra khỏi phạm vi hoạt động (scope).

- Reference Types (Kiểu tham chiếu): Bộ nhớ được chia làm hai phần rõ rệt.
  + Địa chỉ tham chiếu (con trỏ bộ nhớ) được lưu trữ trên Stack.
  + Dữ liệu thực tế và các thuộc tính của đối tượng luôn được cấp phát và lưu trữ trên vùng nhớ Heap.
  + Vùng nhớ Heap do bộ thu gom rác Garbage Collector (GC) tự động quản lý. GC sẽ quét và thu hồi vùng nhớ khi không còn con trỏ nào tham chiếu đến đối tượng đó.

### 2. Bảng so sánh chi tiết

| Tiêu chí | Value Types | Reference Types |
| --- | --- | --- |
| Các kiểu đại diện | int, float, double, bool, char, struct, enum | class, interface, delegate, string, object, mảng (Array) |
| Vị trí lưu trữ vùng nhớ | Stack (nếu là biến cục bộ) hoặc Heap (nếu nằm trong 1 đối tượng) | Địa chỉ lưu trên Stack, Dữ liệu thực tế lưu trên Heap |
| Cơ chế gán và truyền tham số | Pass-by-value: Tạo ra một bản sao dữ liệu hoàn toàn độc lập | Pass-by-reference: Sao chép địa chỉ tham chiếu, cùng trỏ đến 1 vùng nhớ |
| Giá trị mặc định | Mang giá trị khởi tạo mặc định (ví dụ 0, false), không nhận null (trừ Nullable) | Mặc định là null khi chưa được khởi tạo qua toán tử new |
| Quản lý và giải phóng bộ nhớ | Tự động giải phóng khi hết phạm vi hoạt động (scope) | Do bộ thu gom rác Garbage Collector (GC) quản lý |


## Câu 2: Tính năng Init-only Properties (init) trong C# 9/10 khác gì so với thuộc tính có set thông thường? Nêu trường hợp sử dụng thực tế

### 1. So sánh sự khác nhau giữa init và set thông thường
- Thuộc tính set thông thường: Cho phép gán, thay đổi và ghi đè giá trị của thuộc tính ở bất kỳ thời điểm nào trong suốt vòng đời của đối tượng. Dữ liệu có thể bị thay đổi tự do từ bên ngoài (Read-Write).
- Thuộc tính init (Init-only property): Chỉ cho phép gán giá trị một lần duy nhất trong quá trình khởi tạo đối tượng (thông qua Constructor hoặc cú pháp Object Initializers). Sau khi quá trình khởi tạo hoàn tất, thuộc tính sẽ trở thành chỉ đọc (Read-Only) và không thể sửa đổi giá trị.

### 2. Trường hợp sử dụng thực tế
- Thiết kế đối tượng bất biến (Immutable Objects): Giúp bảo vệ dữ liệu không bị vô tình chỉnh sửa ở các tầng xử lý phía sau, đảm bảo an toàn luồng (Thread-safety) trong các ứng dụng đa luồng.
- Thiết kế Data Transfer Object (DTO) và API Models: Khi nhận dữ liệu từ Database hoặc Request API, DTO chỉ cần gán giá trị ban đầu và cần giữ nguyên tính toàn vẹn dữ liệu trong suốt luồng xử lý.
- Cú pháp khởi tạo linh hoạt: Cho phép lập trình viên sử dụng Object Initializer syntax (ví dụ: new Person { Name = "Alice", Age = 25 }) ngắn gọn thay vì phải tạo quá nhiều Constructor overload với nhiều tham số phức tạp.


## Câu 3: Phân biệt sự khác nhau giữa phương thức virtual ở lớp cha và phương thức override ở lớp con khi triển khai tính Đa hình (Polymorphism)

### 1. Vai trò của từng từ khóa
- Phương thức virtual ở lớp cha: Cho biết phương thức này cho phép các lớp con kế thừa có thể ghi đè lại. Lớp cha sẽ cung cấp sẵn một bản triển khai mặc định (default implementation) cho phương thức này.
- Phương thức override ở lớp con: Khai báo bản triển khai mới ở lớp con để thay thế hoàn toàn hành vi của phương thức virtual từ lớp cha đối với đối tượng của lớp con đó.

### 2. Cơ chế thực thi Đa hình tại thời điểm chạy (Runtime Polymorphism)
- Khi gọi phương thức thông qua một biến kiểu lớp cha nhưng tham chiếu đến đối tượng thực tế của lớp con (ví dụ: BaseClass obj = new DerivedClass();):
  + Trình biên dịch và môi trường CLR sẽ tra cứu bảng phương thức ảo (Virtual Method Table / V-Table) tại thời điểm chương trình đang chạy.
  + Nhờ từ khóa override, chương trình sẽ thực thi đúng phương thức đã được định nghĩa tại lớp con (DerivedClass) thay vì thực thi phương thức mặc định của lớp cha.
  + Nếu lớp con không dùng từ khóa override mà dùng từ khóa new, phương thức ở lớp con sẽ chỉ ẩn phương thức lớp cha (Method Hiding) chứ không ghi đè, làm mất đi tính đa hình khi gọi qua biến kiểu lớp cha.


## Câu 4: Tại sao một thành phần được khai báo là static trong Lớp (Class) lại không thể truy xuất thông qua một thể hiện (Object Instance) được tạo bằng toán tử new?

### 1. Bản chất quản lý bộ nhớ và cấp độ thuộc tính
- Thành phần static thuộc về Cấp độ Lớp (Type Level) chứ không thuộc về Cấp độ Thể hiện (Instance Level).
- Bộ nhớ cho các thành phần static được cấp phát một lần duy nhất khi Lớp được nạp vào bộ nhớ (Class Loading Process), tồn tại độc lập và trước khi bất kỳ đối tượng nào được tạo ra bằng toán tử new.
- Mỗi thể hiện đối tượng (Instance) tạo ra bằng toán tử new chỉ quản lý vùng nhớ dữ liệu riêng của nó trên Heap, hoàn toàn không sở hữu riêng bản sao nào của thành phần static.

### 2. Nguyên tắc thiết kế ngôn ngữ C#
- Tránh sự mơ hồ về mặt ngữ nghĩa (Code Readability): Nếu cho phép truy xuất instance.StaticMember, người đọc code sẽ dễ nhầm tưởng rằng đây là thuộc tính riêng biệt của đối tượng đó.
- Đảm bảo tính tường minh (Explicit Code): C# bắt buộc lập trình viên phải truy xuất thông qua tên Lớp (ClassName.StaticMember). Điều này giúp phân biệt rõ ràng giữa dữ liệu dùng chung của toàn bộ Class và dữ liệu riêng biệt của từng Instance, giảm thiểu các lỗi logic trong quá trình phát triển phần mềm.
