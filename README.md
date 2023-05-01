# PayPoint тестовое задание

## Задание 1. Генерация приватного ключа и адреса
Использовал библиотеку TronNet, оболочку Tron.Net от [stoway](https://github.com/stoway/TronNet)

### Использование: <br>
![image](https://user-images.githubusercontent.com/61066851/235483395-30d1b824-559c-4989-ac82-95b43d169b6e.png)

### Консольный вывод: <br>
![image](https://user-images.githubusercontent.com/61066851/235484895-4684fea3-4d90-496c-a5dc-844a78fd7737.png)

#### Корректность генерации ключей проверял при помощи создания аккаунтов <br>
![image](https://user-images.githubusercontent.com/61066851/235485670-bfa4f8c7-ff6a-48c0-b252-b95966618bdc.png)

## Задание 2. Получение баланса монеты TRC-20 (а конкретно USDT)

### Использование:
![image](https://user-images.githubusercontent.com/61066851/235486575-699ffcaf-85b2-406f-a373-8ca6c83910af.png)

### Консольный вывод:
![image](https://user-images.githubusercontent.com/61066851/235490662-fb3536e7-71f8-4307-beed-07f722c10254.png)

### Кошелёк: 
![image](https://user-images.githubusercontent.com/61066851/235490945-f61e8fd1-333a-4401-8fe2-e1621bed4c06.png)

## Задание 3. Отправка монет (Вывод: хеш транзакции)

### Использование:
![image](https://user-images.githubusercontent.com/61066851/235491555-f086f655-4c45-49ed-902d-8c02ccab04a0.png)

### Консольный вывод:
![image](https://user-images.githubusercontent.com/61066851/235492432-c80c1910-62b5-4de7-9414-201fe8e63221.png)

### Кошелёк отправителя:
#### До:
![image](https://user-images.githubusercontent.com/61066851/235492164-39dc4c2d-6177-425d-9955-e755fa2aa085.png)
#### После:
![image](https://user-images.githubusercontent.com/61066851/235492509-9797133a-70ae-4c1d-95d3-2fc9bb690c3e.png)

#### Кошелёк получателя:
#### До:
![image](https://user-images.githubusercontent.com/61066851/235492253-25f8465c-c775-4184-8c36-8fdde4986b87.png)
#### После:
![image](https://user-images.githubusercontent.com/61066851/235492570-f7aad1e0-4fa9-4044-9121-9a8b2eb460f3.png)

## Задание 4. Получение истории операций в период между датами(Вывод: JSON или массив транзакций)
### Использование: <br>
![image](https://user-images.githubusercontent.com/61066851/235499404-22cd83db-d812-46c1-90cf-582afce4981d.png)

### Консольный вывод:
![image](https://user-images.githubusercontent.com/61066851/235493560-f4f7156a-5905-43d7-8d52-64a7735f61d6.png)

### JSON HANDLER:
![image](https://user-images.githubusercontent.com/61066851/235498598-65a1ec1f-60aa-4e77-a805-e1e9be2a92c6.png)

## Детали реализации:
```Kotlin
За отправку запросов отвечает класс TronApiRequest
```
```Kotlin
За генерацию ключа, адреса и подпись транзакции отвечает класс CryptoOps
```
```Kotlin
Класс RequestParser получает информацию из JSON ответов
```
```Kotlin
Класс Tools отвечает за работу со строками и значениями (единицами токенов)
```
```Kotlin
Класс Base58 отвечает за работу со строками BASE58 (Логично)
```
```Kotlin
Перечисление TRC20Token содержит виды токенов стандарта TRC20
```

## Не получилось:
1) Перевод валюты реализован только для токена TRX
2) Реализация получения баланса неверная, тк значения не всегда помещаются в decimal
Предположение:
Нужно было использовать uint256 или каким-то образом получать информацию о токене при помощи адреса контракта<br>
Если переставить точку на n-позиций с конца результата, где n - параметр decimal(лежит внутри информации о токене), получается корректное число.

Причина: Недостаток информации о возможностях api, последовательности ABI шифрования в смарт контрактах, ограничение по времени.
(На одном форуме было сказано, что официальная документация это образец не самого лучшего перевода с китайского и что в оригинале многие моменты гораздо прозрачнее, но китайский я не знаю)
