# 💈 Sleeping Barber Problem - C# Implementation

A thread-safe implementation of the classic **Sleeping Barber Problem**, demonstrating concurrency control, synchronization primitives, and resource management in C# .NET.

This project serves as a practical study of **Producer-Consumer** patterns and **Inter-process Communication (IPC)** mechanisms within a multi-threaded environment.

## 🚀 Overview

The Sleeping Barber problem is a classic inter-process communication and synchronization problem between multiple operating system processes. The problem is analogous to that of keeping a barber working when there are customers, resting when there are none, and doing so in an orderly manner.

**Scenario:**
- The shop has a waiting room with `N` chairs.
- If there are no customers, the barber goes to sleep.
- If a customer arrives and the barber is sleeping, the customer wakes the barber up.
- If a customer arrives and the barber is busy:
    - If there is an empty chair, the customer sits and waits.
    - If there are no empty chairs, the customer leaves.

## 🛠 Tech Stack & Key Concepts

This project is a deep dive into low-level concurrency and synchronization in C#. It intentionally avoids high-level Task Parallel Library (TPL) abstractions to showcase mastery over core .NET threading primitives.

### 🧵 Advanced Multithreading & Concurrency

- **Manual Thread Lifecycle:** Utilizes `System.Threading.Thread` to simulate independent worker processes (Barbers), demonstrating control over thread priorities and background execution.

- **Signaling with** `SemaphoreSlim`**:** Implements a highly efficient producer-consumer signal. Barbers remain in a low-overhead wait state until a customer "releases" the semaphore, eliminating CPU-intensive busy-waiting.

- **Critical Section Management:** Employs the `lock` (Monitor) keyword to enforce strict mutual exclusion on shared resources like the `Queue<Customer>`, preventing race conditions and ensuring data integrity.

### 🏗 Architecture & Resource Control

- **Producer-Consumer Pattern:** Architected as a decoupled system where `Customer` instances act as transient producers and `Barber` threads as persistent consumers.

- **Graceful Termination Flow:** Implements robust thread teardown using `CancellationTokenSource`. This ensures that even during asynchronous cancellation, all threads are joined and managed resources are disposed of without memory leaks.

- **Thread-Safe UI Logging:** Features a dedicated `ConsoleHelper` with internal locking to prevent interleaved or corrupted console output, a common pitfall in multi-threaded CLI applications.

- **Deterministic Cleanup:** Adheres to the `IDisposable` pattern to manually release unmanaged-like synchronization primitives, reflecting professional memory management standards.



## 📂 Project Structure

- **`BarberShop.cs`**: The orchestrator. Manages the `Queue<Customer>`, the `SemaphoreSlim` for signaling, and the barbers.
- **`Barber.cs`**: Represents the worker thread. It waits on the semaphore and processes customers.
- **`Customer.cs`**: Represents the task/data unit. Tries to acquire a lock to enter the queue.
- **`ConsoleHelper.cs`**: A thread-safe static helper to ensure console output remains readable (preventing race conditions on the output stream).
- **`Program.cs`**: Entry point. Parses arguments, initializes the simulation, and spawns customers randomly.

## ⚙️ How It Works (Code Highlights)

### 1. The Semaphore Signal
Instead of busy-waiting, the Barber waits for a signal. `SemaphoreSlim` is initialized with 0.
```csharp
// BarberShop.cs

public Customer TakeCustomer(CancellationToken token)
{
    // Barber sleeps here until a customer releases the semaphore
    _customerSemaphore.Wait(token);

    lock (_lockObject)
    {
        Customer customer = _customerQueue.Dequeue();
        PrintStatus();
        return customer;
    }
}
```

### 2. Thread-Safe Queueing

When a customer arrives, we try to lock the queue. If it's full, the customer leaves immediately (Non-blocking attempt).

```csharp
// BarberShop.cs

public bool TryEnter(Customer customer)
{
    lock (_lockObject)
    {
        if (_customerQueue.Count >= _customerQueue.Capacity)
            return false; // Shop is full

        _customerQueue.Enqueue(customer);
        _customerSemaphore.Release(); // Wake up a barber
        return true;
    }
}
```

### 3. Graceful Cancellation

Using CancellationToken ensures that the simulation doesn't leave zombie threads when the shop closes.
```csharp
// Barber.cs

try
{
    // ... work loop
    _shop.TakeCustomer(_token); // Throws OperationCanceledException if token is cancelled
}
catch (OperationCanceledException)
{
    // Clean exit
}
```

## 🖥️ How to Run

### 1. Clone the repository:
```bash
git clone https://github.com/duhanayan/Sleeping-Barber-CSharp.git
```

### 2. Navigate to the project folder:
```bash
cd SleepingBarber
```

### 3. Run with default parameters (3 Seconds, 2 Barbers, 3 Chairs):
```bash
dotnet run
```

### 4. Run with custom parameters:
```bash
# 5 Seconds, 4 Barbers, 10 Chairs
dotnet run -- 5 4 10
```

## 📊 Sample Output
```bash
# 3 Seconds, 2 Barbers, 3 Chairs
dotnet run
```
<pre>
<code>3 seconds simulation starting with: 2 Barbers, 3 Chairs.</code>
<code>Note: Each barber has a random shave duration (100-500ms).</code>
<code> </code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Barber (1)</span> has started working.</code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Barber (1)</span> is sleeping.</code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Barber (2)</span> has started working.</code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Barber (2)</span> is sleeping.</code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Customer (1)</span> arrived.</code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Customer (1)</span> sat on the 1. chair.</code>
<code><span style="color: gray;">1</span> 2 chairs are empty out of 3.</code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Customer (1)</span> woke Barber (1) up.</code>
<code><span style="color: gray;">1</span> <span style="color: yellow;">Barber (1)</span> has started shaving Customer (1).</code>
<code><span style="color: gray;">116</span> <span style="color: yellow;">Customer (2)</span> arrived.</code>
<code><span style="color: gray;">116</span> <span style="color: yellow;">Customer (2)</span> sat on the 1. chair.</code>
<code><span style="color: gray;">116</span> 2 chairs are empty out of 3.</code>
<code><span style="color: gray;">116</span> <span style="color: yellow;">Customer (2)</span> woke Barber (2) up.</code>
<code>...</code>
<code><span style="color: gray;">6263</span> <span style="color: yellow;">Barber (2)</span> has stopped working.</code>
<code><span style="color: gray;">6263</span> <span style="color: yellow;">Barber (1)</span> has stopped working.</code>
</pre>

# 📄 License
MIT License - feel free to use this for learning or your portfolio!
