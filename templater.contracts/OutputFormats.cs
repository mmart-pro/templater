﻿namespace templater.contracts
{
    /// <summary>
    /// Форматы выходного файла
    /// </summary>
    public enum OutputFormats
    {
        /// <summary>
        /// Конвертировать результат в PDF, в случае нескольких входных шаблонов, результат объединяется в один pdf-файл
        /// Источником могут выступать любые доступные шаблоны
        /// </summary>
        PDF = 0,

        /// <summary>
        /// Сжать в zip-архив
        /// Источником могут выступать любые доступные шаблоны
        /// </summary>
        ZIP = 1,

        /// <summary>
        /// Документ объединяется путём создания новых листов в книге
        /// Источником могут выступать только xlsx-шаблоны
        /// </summary>
        XLSX = 2,

        /// <summary>
        /// Документ объединяется путём добавления листов в общий документ
        /// Источником могут выступать только docx-шаблоны
        /// </summary>
        DOCX = 3
    }
}