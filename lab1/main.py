import matplotlib.pyplot as plt
import numpy as np

def lehmer_random(a, R0, m, n):
    random_numbers = []
    for _ in range(n):
        R0 = (a * R0) % m
        random_numbers.append(R0 / m)
    return random_numbers

a = int(input("Введите параметр a (натуральное число): "))
R0 = int(input("Введите начальное значение R0 (натуральное число): "))
m = int(input("Введите параметр m (натуральное число): "))
n = int(input("Введите количество случайных чисел для генерации: "))

random_sequence = lehmer_random(a, R0, m, n)

# Запись результатов в файл
with open("results.txt", "w") as file:
    file.write("Случайная последовательность:\n")
    for i, random_number in enumerate(random_sequence, start=1):
        file.write(f"Случайное число {i}: {random_number}\n")

    # Проверка равномерности по косвенным признакам
    pairs = [(random_sequence[i], random_sequence[i+1]) for i in range(0, len(random_sequence)-1, 2)]
    count = sum(1 for x1, x2 in pairs if (x1**2 + x2**2) < 1)

    relative_frequency = count / (n // 2)
    file.write(f"\nОтносительная частота попадания в четверть круга: {relative_frequency}\n")

    # Расчет оценок
    mean = np.mean(random_sequence)
    variance = np.var(random_sequence)
    stddev = np.std(random_sequence)

    # Запись оценок в файл
    file.write("Математическое ожидание: {}\n".format(mean))
    file.write("Дисперсия: {}\n".format(variance))
    file.write("Среднее квадратическое отклонение: {}\n".format(stddev))

    # Гистограмма
    plt.hist(random_sequence, bins=20)
    plt.xlabel("Число")
    plt.ylabel("Частота")
    plt.show()

    # Вычисление длины периода
    count = int(5e6)
    numbers = lehmer_random(a, R0, m, count)

    last_number = numbers[-1]
    prelast_number_index = count - 2
    while numbers[prelast_number_index] != last_number:
        prelast_number_index -= 1

        if prelast_number_index == 0:
                periodLength = -1
                aperiodLength = -1

    period_length = count - 1 - prelast_number_index

    i = 0
    while numbers[i] != numbers[i + period_length]:
        i += 1

    periodLength = period_length
    aperiodLength = i + period_length

print(f"Длина периода:", {periodLength})
print(f"Длина участка апериодичности:", {aperiodLength})