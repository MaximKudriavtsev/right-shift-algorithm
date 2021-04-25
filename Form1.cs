using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DCB_transform {               //К этой 
    public partial class Form1 : Form { //и этой закрывающие скобки в конце текста!

        public Form1() {
            InitializeComponent();
        }
//==============================================================
        /*  Проект на моём компе находится здесь:
         *  D:\Visual Studio 2012\Projects\C#\DCB_transform\DCB_transform\bin\Debug\DCB_transform.exe
         *  При переносе в другое место не загружается!
         */
        private void button1_Click(object sender, EventArgs e) {

            textBox1.Text = "";
            textBox2.Text = "";
            label1.Text = "0";
            label2.Text = "0";
           //// this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox2.BackColor = System.Drawing.Color.White;
            if (textBox1.CanFocus) {textBox1.Focus();} 
        } //  button1_Click

        private void textBox1_TextChanged(object sender, EventArgs e) {
            label1.Text = textBox1.TextLength.ToString();
            textBox2.Text = "";
            this.textBox2.BackColor = System.Drawing.Color.White;
            label2.Text = "0";
            label3.Text = "Dec";
            label4.Text = "Convert";
            label5.Text = "Hex";
            label6.Text = "Sum";
        } // textBox1_TextChanged

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            //Ввод 10-чных символов с клавиатуры
            //защита от ввода посторонних символов
            char simbol = e.KeyChar;
            if ((simbol < 48 || simbol > 59)       //если не цифры 0-9
//                && (e.KeyChar < 65 || e.KeyChar > 70)  //и не A-F
//                && (e.KeyChar < 92 || e.KeyChar > 102) //и не a-f
                && simbol != 8                         //и не BackSpase ?
                && simbol != 3                         // ctrl-C
                && simbol != 0x16                      // ctrl-V
                // && simbol != 0x0D  //переводит строку, а надо преобраование по button2_Click
                || simbol == 0x5C  //92                //обратная слэшь 92
                || simbol == 0x5D                      // ] 
                || simbol == 0x5E                      // ^
                || simbol == 0x5F                      // _
                || simbol == 0x60                      // `  над Ё
                //              || simbol == 0x09      // HT (Горизонтальная табуляция
                //срабатывает как переход между окнами
                || simbol == 0x3A                      // :
                || simbol == 0x3B                      // ;
            ) //конец условия if, //то:
            {
                e.Handled = true;                       //событие Box1_KeyPress обработано
                this.textBox1.BackColor = System.Drawing.Color.Red; //нажата не 16-ричная клавиша,
                //о чём красный сигнал, и cимвол в textBox1 не заносится
            }//end if (    (number < 48 ...
        } //textBox1_KeyPress

        private void textBox1_KeyUp(object sender, KeyEventArgs e) {
            this.textBox1.BackColor = System.Drawing.Color.White;   //при отпускании клавиши 
            //снова белый сигнал, который мог стать красным в textBox1_KeyPress
        } //textBox1_KeyUp

//-----------=======================        
        private void button2_Click(object sender, EventArgs e) {

/*
 * Алгоритм обратного DCB-преобразования одной строкой:
 * после сдвига вправо вычитать из декады число 3, если декада больше 7.
 *
 * Подробнее алгоритм обратного (DCB) преобразования смотри в статье
 * "Преобразование двоично-десятичного в двоичный код больших целых чисел, 
 * у которых количество цифр – сотни и тысячи"
 * Этот проект и программа по адресу
 *            D:\Visual Studio 2012\Projects\C#\DCB_transform
 *
 * Здесь обратное преобразование, 
 * а о прямом преобразовании читай в статье
 * "Двоично-десятичное преобразование больших целых чисел,
 * у которых количество цифр – многие десятки, сотни и тысячи"
 * Проект и программа прямого преобразования по адресу 
 *           D:\Visual Studio 2012\Projects\C#\WindowsFormsApplication6
 */
          //  textBox2.Text = "";
            this.textBox2.BackColor = System.Drawing.Color.Yellow;
            //Преобразуемое число находится в textBox1.Text 
            int Len = textBox1.TextLength;  //кол-во символов-цифр-декад входного Дв-Дес числа 
            label1.Text = Len.ToString();
            //Результат преобразования окажется в textBox2.Text, который до начала 
            //преобразования должен быть обнулён
            

            if (Len > 0) { // закрывающая скобка в конце этого метода (button2_Click)
                // без этого условия при нажатии на button2 индексы массивов вне диапазона

                //****************************
                //Определение времени преобразования
                // Пример 10 из https://coderoad.ru/2821040
                DateTime dtstart = DateTime.Now;
                // Do some work //- преобразование. Две строки, закомментированные ниже, в конце button2_Click
                //TimeSpan timeDiff = DateTime.Now - start;
                //label3.Text = timeDiff.TotalMilliseconds.ToString();
                //****************************

                //lg(16) ≈ 1,204119982655924780854955578898 //из калькулятора

                double log16 = Math.Log10(16);  //в компе
                // log16 = 1,20411998265592 - вычислено в предыдущей строке 
                //такой точности хватит на 50 триллионов цифр в числе.      
                //textBox2.Text = log16.ToString();
                //log16 = 1,20412, если float  //этого логарифма хватит на количество цифр гораздо 
                //меньше: ~10 млн цифр по правилам округления, 
                //или ~50000 цифр формально
                //n = int(m/lg(16)) + 1
                //m = len;

                //ОПРЕДЕЛЕНИЕ РАЗМЕРОВ МАССИВОВ

                

                int dighex, digdec; //количество цифр в 16-ричном и 10-тичном числах
                digdec = Len;       // десятичные цифры берём из textBox1.Text
                dighex = (int)(digdec / log16) + 1; //к-во 16ричных определяем по этой ф-ле
                label2.Text = dighex.ToString();

                //ОБЪЯВЛЕНИЕ БАЙТОВЫХ МАССИВОВ
                int mbytedec = digdec / 2;              //вычисление размера входного массива
                if ((digdec & 1) != 0) {                //если к-во символов нечетно, 
                    ++mbytedec;                         //размер массива увеличивается на 1
                }
                byte[] masdec = new byte[mbytedec];     // входной (masdec) массив

                int nbytehex = dighex / 2;              //размер выходного массива
                if ((dighex & 1) != 0) {                //если к-во символов нечетно, 
                    ++nbytehex;                         //размер увеличивается на 1
                }
                byte[] mashex = new byte[nbytehex];     // выходной (mashex) массив

                //ОБНУЛЕНИЕ МАССИВОВ
                ////Обнуляем входной массив перед заполнением
                int ii = mbytedec - 1;
                while (ii >= 0) {
                    masdec[ii] = 0;
                    ii--;
                }
                //Перед преобразованием выходной массив mashex должен быть обнулён:
                ii = nbytehex - 1;
                while (ii >= 0) {
                    mashex[ii] = 0;
                    ii--;
                } //while (ii >= 0); 

                //=====================================
                //// ЗАПОЛНЕНИЕ ВХОДНОГО МАССИВА masdec

                //заполнение байтового вх массива masdec подекадно цифрами, 
                //взятыми из textBox1.Text
                //к-во символов текста равно к-ву декад в массиве.
                //перебор всего введённого текста по две декады в байте массива
                //в следующем цикле
                // digdec = Len;       // количество цифр в textBox1.Text 

                byte tetra0 = 0, tetra1 = 0, tet = 0;
                //////////////////
                //Len - фактическая длина текста textBox1.Text
                int jj; //= Len - 1 - ii; //индекс декады в массиве (можно обойтись, но для наглядности)
                int ik;		              //индекс декады в байте
                int nb;		              //индекс байта в массиве

                ii = Len - 1;		      //индекс символа в строке  textBox1
                while (ii >= 0) {
                    
                    jj = Len - 1 - ii;
                    ik = jj % 2;
                    nb = jj / 2;

                    char ch = textBox1.Text[ii];
                    switch (ch) {
                        case '0':
                            tet = 0x00;
                            break;
                        case '1':
                            tet = 0x01;
                            break;
                        case '2':
                            tet = 0x02;
                            break;
                        case '3':
                            tet = 0x03;
                            break;
                        case '4':
                            tet = 0x04;
                            break;
                        case '5':
                            tet = 0x05;
                            break;
                        case '6':
                            tet = 0x06;
                            break;
                        case '7':
                            tet = 0x07;
                            break;
                        case '8':
                            tet = 0x08;
                            break;
                        case '9':
                            tet = 0x09;
                            break;
                        default:    //такого не должно быть! Аварийное вкл цвета
                            this.textBox2.BackColor = System.Drawing.Color.Violet;
                            break;
                    }//switch
                    //====================
                    //  1 2 3 4 5	- Len - к-во симв в строке 

                    //  0 1 2 3 4	- ii -  индексы символов в строке   (счёт слева)
                    //  4 3 2 1 0   - jj -  индексы декад в массиве     (счёт справа) 
                    //  0 1 0 1 0   - ik -  индексы декад в байте       (счёт справа)
                    //--- --- ---	- байты массива подчёркнуты
                    // 2   1   0	- nb -  индексы байтов в массиве    (счёт справа)
                    //====================
                    //упаковка в байт двух цифр, взятых из textBox1.Text[ii]
                    if (ik == 0) {
                        tetra0 = tet;
                    } else {
                        tetra1 = (byte)(tet << 4);  // (tet * 16);
                        tetra0 |= tetra1;
                    }
                    masdec[nb] = tetra0;    //в nb-й байт массива упакованы две цифры

                    ii--;   //перебор textBox1.Text[ii] символов 10-тичных цифр
                }//end while (ii < Len)  - массив mashex[] заполнен 
                //Конец заполнения входного массива
//
                //dtfilling
                DateTime dtfilling = DateTime.Now;

                ////=================================================
                // ПРЕОБРАЗОВАНИЕ МАССИВА ДВОИЧНО-ДЕСЯТИЧНЫХ ЦИФР В МАССИВ ШЕСТНАДЦАТИРИЧНЫХ
                // (ОБРАТНОЕ ПРЕОБРАЗОВАНИЕ)
                // АЛГОРИТМ ПРАВОГО СДВИГА

 //В цикле по ih производится в каждом проходе:
//1.Сдвиг вправо на один разряд всех тетрад выходного массива mashex с переносом между байтами массива, 
//2.Перенос из младшего бита младшей декады в старший бит старшей тетрады, 
//3.Сдвиг вправо на один разряд всех декад входного массива masdec с переносом между байтами массива, 
//4.Коррекция декад: после сдвига вправо вычитать из декады число 3, если декада больше 7.

                int ktet = dighex;      // к-во тетрад в этом блоке
                int nhex = nbytehex;    // необходимое к-во байтов выходного массива - размер массива
                int kdec = digdec;      // к-во декад  
                int mdec = mbytedec;    // к-во байтов в дв-дес массиве - размер массива
                byte leftbit = 0, rightbit = 0;

                 int ih = 0;            //long счётчик бит ih и ktih - long?, иначе количество тетрад уменьшится   
                 int ktih = ktet * 4;   //вчетверо от максимального

                while (ih < ktih)       //ih счётчик к-ва двоичных разрядов входного числа
                {
//1.Сдвиг вправо на один разряд всех тетрад выходного массива mashex с переносом между байтами массива, 
                    ii = 0;
                    while (ii < (nhex - 1)) {
                        mashex[ii] >>= 1;
                        // mashex[ii].7 = ddd.0;    //таков перенос одной строкой в микроконтроллере,
                                                    //там разрешены битовые операции,
                        rightbit = mashex[ii + 1];  //а это до конца цикла - перенос в этой пр-ме
                        rightbit &= 0x01;
                        if (rightbit != 0) {
                            leftbit = 0x80;
                        } else {
                            leftbit = 0;
                        }
                        mashex[ii] += leftbit;
                        ii++;
                    } //  while (ii < nhex)
                    mashex[nhex - 1] >>= 1;     //cт байт вых массива тетрад - 16-ричных цифр
                  
//2.перенос из младшего бита младшей ДЕКАДЫ в старший бит старшей ТЕТРАДЫ, т.е. перенос из одного массива в другой
                    rightbit = masdec[0];
                    rightbit &= 0x01;           //младший бит младшей декады
                    if (rightbit != 0) {        //если младший бит младшей декады =1, то перенос будет 1
                        if ((dighex & 1) != 0)  //если к-во тетрад нечётно,
                            leftbit = 0x08;     //то перенос 1 будет в 3 бит
                        else
                            leftbit = 0x80;     //если чётно, то перенос 1 будет в 7 бит
                    } else {
                        leftbit = 0;            //Если младший бит младшей декады =0, то перенос будет 0
                    }
                    mashex[nhex - 1] += leftbit;//вставляем перенос в старшую тетраду mashex 

//3.сдвиг вправо на один разряд всех декад входного массива masdec с переносом между байтами массива     
                    ii = 0;
                    while (ii < (mdec - 1)) {
                        masdec[ii] >>= 1;
                        rightbit = masdec[ii + 1]; 
                        rightbit &= 0x01;
                        if (rightbit != 0) {
                            leftbit = 0x80;
                        } else {
                            leftbit = 0;
                        } //if (rightbit != 0)
                        masdec[ii] += leftbit;
                        ii++;
                    } //  while (ii < nhex)
                    masdec[mdec - 1] >>= 1;     //cт байт вых массива тетрад - 16-ричных цифр

                    //switch (ih) { //отладочный вывод, благодаря ему сразу увидел последнюю описку
                    //    case 0:
                    //        numericUpDown8.Value = masdec[1];
                    //        numericUpDown7.Value = masdec[0];
                    //        numericUpDown6.Value = mashex[1];
                    //        numericUpDown5.Value = mashex[0];
                    //        break;
                    //    case 1:
                    //        numericUpDown16.Value = masdec[1];
                    //        numericUpDown15.Value = masdec[0];
                    //        numericUpDown14.Value = mashex[1];
                    //        numericUpDown13.Value = mashex[0];
                    //        break;
                    //}

//4.коррекция декад: после сдвига вправо вычитать из декады число 3, если декада больше 7.
                    for (ii = mdec - 1; ii >= 0; ii--) {
                        byte dualdec, dec0;
                        dualdec = masdec[ii];

                        dec0 = (byte)(dualdec & 0x0F);
                        if (dec0 > 7)        //если младшая декада больше 7,
                        { dec0 -= 3; }       //то из неё вычитается 3 - это и есть коррекция 

                        dualdec &= 0xF0;     //Выделение из байта старшей декады,
                        if (dualdec > 0x70)  //если старшая декада больше 7,
		                { dualdec -= 0x30; } //то из неё вычитается 3 

                        dualdec |= dec0;
                        masdec[ii] = dualdec;//после коррекции - в тот же байт массива
                    } //for 

                    ih++;   //ih счётчик к-ва двоичных разрядов входного числа  во всех тетрадах
                }//	while (ih > 0)

                //dtconvert
                DateTime dtconvert = DateTime.Now;

                //*************************
                //ВЫВОД результата В textBox2.Text
                //две тетрады выходного массива преобразуются в символы-цифры и добавляются к тексту
                //в цикле побайтно
                string stringout = "";
                char cdig = '0';
                ii = nhex - 1;              //начиная со старшего байта массива - младшей цифры числа
                while (ii >= 0) {
                    tetra1 = mashex[ii];
                    tetra1 &= 0xF0;         //самая старшая цифра может оказаться начальным нулём,
                    tetra1 >>= 4;           //её здесь выводим, а после цикла вывода удаляем 
                                            //начальные нули
                    switch (tetra1) {
                        case (byte)(0):
                            cdig = '0';
                            break;
                        case 0x01:
                            cdig = '1';
                            break;
                        case 0x02:
                            cdig = '2';
                            break;
                        case 0x03:
                            cdig = '3';
                            break;
                        case 0x04:
                            cdig = '4';
                            break;
                        case 0x05:
                            cdig = '5';
                            break;
                        case 0x06:
                            cdig = '6';
                            break;
                        case 0x07:
                            cdig = '7';
                            break;
                        case 0x08:
                            cdig = '8';
                            break;
                        case 0x09:
                            cdig = '9';
                            break;
                        case 0x0A:
                            cdig = 'A';
                            break;
                        case 0x0B:
                            cdig = 'B';
                            break;
                        case 0x0C:
                            cdig = 'C';
                            break;
                        case 0x0D:
                            cdig = 'D';
                            break;
                        case 0x0E:
                            cdig = 'E';
                            break;
                        case 0x0F:
                            cdig = 'F';
                            break;
                        default:    //такого не должно быть! Поэтому аварийный фиолетовый сигнал
                            this.textBox2.BackColor = System.Drawing.Color.Violet;
                            break;
                    }//switch

                    stringout += cdig;          //старшая цифра байта в текст
                  //  textBox2.Text = textBox2.Text + cdig;   //такой способ вывода на два порядка 
                                                                //медленнее, чем в прдыдущем операторе
                    tetra0 = mashex[ii];                    //из того же байта массива
                    tetra0 &= 0x0F;                         //выделяется младшая цифра,

                    switch (tetra0) {                       //преобразуется в символ
                        case (byte)(0):
                            cdig = '0';
                            break;
                        case 0x01:
                            cdig = '1';
                            break;
                        case 0x02:
                            cdig = '2';
                            break;
                        case 0x03:
                            cdig = '3';
                            break;
                        case 0x04:
                            cdig = '4';
                            break;
                        case 0x05:
                            cdig = '5';
                            break;
                        case 0x06:
                            cdig = '6';
                            break;
                        case 0x07:
                            cdig = '7';
                            break;
                        case 0x08:
                            cdig = '8';
                            break;
                        case 0x09:
                            cdig = '9';
                            break;
                        case 0x0A:
                            cdig = 'A';
                            break;
                        case 0x0B:
                            cdig = 'B';
                            break;
                        case 0x0C:
                            cdig = 'C';
                            break;
                        case 0x0D:
                            cdig = 'D';
                            break;
                        case 0x0E:
                            cdig = 'E';
                            break;
                        case 0x0F:
                            cdig = 'F';
                            break;
                        default:    //такого не должно быть!
                            this.textBox2.BackColor = System.Drawing.Color.Violet;
                            break;
                    }//switch

                    stringout += cdig;      //и добавляется в текст по окончании цикла
                    //  textBox2.Text = textBox2.Text + cdig;  //такой способ вывода на два порядка 
                                                                //медленнее, чем в предыдущем операторе                  
                    ii--;
                } //while (ii >= 0) конец вывода в textBox2.Text

                textBox2.Text = stringout;

                //dtstring
                DateTime dtstring = DateTime.Now;


                textBox2.Text = textBox2.Text.TrimStart('0');       //Удаление начальных нулей

                if (textBox1.Text == "0") { textBox2.Text = "0"; }  //нулевой выход на нулевой вход

                label2.Text = textBox2.TextLength.ToString();       //количество цифр выходного числа

                //********* Заполнение таймеров
                TimeSpan tsfilling = dtfilling - dtstart;
                label3.Text = tsfilling.TotalSeconds.ToString();

                TimeSpan tsconvert = dtconvert - dtfilling;
                label4.Text = tsconvert.TotalSeconds.ToString();

                TimeSpan tsstring = dtstring - dtconvert;
                label5.Text = tsstring.TotalSeconds.ToString();

                TimeSpan spsum = dtstring - dtstart;       //dtstart = DateTime.Now в начале button2_Click
                String strsum = spsum.TotalSeconds.ToString();          //Секунды и 7 десятичных знаков
                int lendtsum = strsum.Length;                             //Длина строки
                label6.Text = strsum.Substring(0, lendtsum /*- 4*/) + " sec"; //Строка без последних 4 десятичных знаков
                                                                              //хотел укоротить, но оказалось, не стоит
            }//if (Len != 0) открывающая скобка { стоит в самом верху этого метода button2_Click

            if (textBox1.CanFocus) { textBox1.Focus(); }

            this.textBox2.BackColor = System.Drawing.Color.White;
        }////button2_Click
        //-------------------------
        private void Form1_Load(object sender, EventArgs e) {
        }
        //-------------------------
    } // public partial class Form1
} // namespace DCB_transform


// интервал между двумя отметками времени - Пример 10 из https://coderoad.ru/2821040
//DateTime start = DateTime.Now;
//// Do some work
//TimeSpan timeDiff = DateTime.Now - start;
//timeDiff.TotalMilliseconds;