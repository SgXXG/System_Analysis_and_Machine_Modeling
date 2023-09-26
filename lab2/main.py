import random
import numpy as np
import matplotlib.pyplot as plt

def generate_uniform(a, b, n):
    return [random.uniform(a, b) for _ in range(n)]

def generate_normal(mean, stddev, n):
    return np.random.normal(loc=mean, scale=stddev, size=n)

def generate_exponential(lambda_param, n):
    while lambda_param <= 0:
        print("Параметр lambda должен быть больше 0.")
        lambda_param = float(input("Введите параметр lambda (должен быть больше 0): "))
    return np.random.exponential(1 / lambda_param, n)

def generate_gamma(shape, scale, n):
    while shape <= 0 or scale <= 0:
        print("Параметры shape и scale должны быть больше 0.")
        shape = float(input("Введите параметр формы (shape): "))
        scale = float(input("Введите параметр масштаба (scale): "))
    return np.random.gamma(shape, 1 / scale, n)

def generate_triangular(a, b, n):
    numbers = []
    for _ in range(n):
        r = random.random()
        x = a + (b - a) * np.sqrt(r)
        numbers.append(x)
    return numbers

def generate_simpson(a, b, n):
    simpson_numbers = []
    for _ in range(n):
        u1 = random.uniform(a/2, b/2)
        u2 = random.uniform(a/2, b/2)
        x = u1+u2
        simpson_numbers.append(x)
    return simpson_numbers


def calculate_statistics(numbers):
    mean = np.mean(numbers)
    variance = np.var(numbers)
    stddev = np.std(numbers)
    return mean, variance, stddev

while True:
    print("Выберите тип распределения:")
    print("1. Равномерное распределение")
    print("2. Гауссовское (нормальное) распределение")
    print("3. Экспоненциальное распределение")
    print("4. Гамма-распределение")
    print("5. Треугольное распределение")
    print("6. Распределение Симпсона")
    print("0. Выйти")

    choice = int(input("Введите номер распределения: "))

    if choice == 0:
        break

    n = int(input("Введите количество случайных чисел: "))

    # Равномерное
    if choice == 1:
        a = float(input("Введите нижний предел: "))
        b = float(input("Введите верхний предел: "))
        numbers = generate_uniform(a, b, n)

        # Вычисление оценок для равномерного распределения
        mean = (a + b) / 2
        variance = ((b - a) ** 2) / 12
        print(f"Ожидаемая оценка математического ожидания (m~): {mean}")
        print(f"Ожидаемая оценка дисперсии (D~): {variance}")
        calculate_statistics(numbers)
    # Гауссовское
    elif choice == 2:
        mean = float(input("Введите среднее значение: "))
        stddev = float(input("Введите стандартное отклонение: "))
        numbers = generate_normal(mean, stddev, n)
        mean, variance, stddev = calculate_statistics(numbers)
        print(f"Оценка математического ожидания (m~): {mean}")
        print(f"Оценка дисперсии (D~): {variance}")
        print(f"Оценка среднего квадратического отклонения (𝜎̃): {stddev}")

    # Экспоненциальное
    elif choice == 3:
        lambda_param = float(input("Введите параметр lambda (должен быть больше 0): "))
        while lambda_param <= 0:
            print("Параметр lambda должен быть больше 0.")
            lambda_param = float(input("Введите параметр lambda (должен быть больше 0): "))
        numbers = generate_exponential(lambda_param, n)

        # Вычисление оценок для экспоненциального распределения
        mean = 1 / lambda_param
        variance = 1 / (lambda_param ** 2)
        print(f"Ожидаемая оценка математического ожидания (m~): {mean}")
        print(f"Ожидаемая оценка дисперсии (D~): {variance}")
        calculate_statistics(numbers)
    # Гамма
    elif choice == 4:
        shape = float(input("Введите параметр формы (shape): "))
        scale = float(input("Введите параметр масштаба (scale): "))
        while shape <= 0 or scale <= 0:
            print("Параметры shape и scale должны быть больше 0.")
            shape = float(input("Введите параметр формы (shape): "))
            scale = float(input("Введите параметр масштаба (scale): "))
        numbers = generate_gamma(shape, scale, n)

        # Вычисление оценок для гамма-распределения
        mean = shape / scale
        variance = shape / (scale ** 2)
        print(f"Ожидаемая оценка математического ожидания (m~): {mean}")
        print(f"Ожидаемая оценка дисперсии (D~): {variance}")
        calculate_statistics(numbers)
    # Треугольное
    elif choice == 5:
        left = float(input("Введите левую границу: "))
        right = float(input("Введите правую границу: "))
        numbers = generate_triangular(left, right, n)
        mean, variance, stddev = calculate_statistics(numbers)

        # Вывод и отображение чисел
        print(f"Последовательность случайных чисел с выбранным распределением ({n} чисел):")
        print(numbers)

        # Вывод и отображение гистограммы
        plt.hist(numbers, bins=20)
        plt.xlabel("Число")
        plt.ylabel("Частота")
        plt.show()

        # Расчет и вывод оценок
        print(f"Оценка математического ожидания (m~): {mean}")
        print(f"Оценка дисперсии (D~): {variance}")
        print(f"Оценка среднего квадратического отклонения (𝜎̃): {stddev}")
    # Симпсона
    elif choice == 6:
        a = float(input("Введите нижний предел: "))
        b = float(input("Введите верхний предел: "))
        numbers = generate_simpson(a, b, n)

        # Вычисление оценок для распределения Симпсона
        mean = (a + b) / 2
        variance = ((b - a) ** 2) / 24

    # Вывод и отображение чисел
    # print(f"Последовательность случайных чисел с выбранным распределением ({n} чисел):")
    # print(numbers)

    # Вывод и отображение гистограммы
    plt.hist(numbers, bins=20)
    plt.xlabel("Число")
    plt.ylabel("Частота")
    plt.show()

    # Расчет и вывод оценок
    print(f"Оценка математического ожидания (m~): {mean}")
    print(f"Оценка дисперсии (D~): {variance}")
    print(f"Оценка среднего квадратического отклонения (𝜎̃): {np.sqrt(variance)}")