﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace templater.contracts
{
    /// <summary>
    /// Таблицы для заполнения
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Шаблон, который будет найден для идентификации шаблонной строки в таблице (фигурные скобки добавляются автоматически)
        /// По умолчанию rowNum
        /// </summary>
        public string RowId { get; set; }

        /// <summary>
        /// Игнорировать нулевые значения (не заполнять нулями)
        /// </summary>
        public bool IgnoreZeroes { get; set; }

        /// <summary>
        /// Строки таблицы, содержащие объекты для замен (структура аналогична replacements)
        /// </summary>
        public IDictionary<string, object>[] Rows { get; set; } = Array.Empty<IDictionary<string, object>>();

        public Table(string rowId, bool ignoreZeroes, IEnumerable<object> rows)
        {
            RowId = rowId;
            IgnoreZeroes = ignoreZeroes;
            try
            {
                Rows = rows.Select(x => x.ToDictionary()).ToArray();
            }
            catch
            {
                throw new ArgumentException("Не удалось преобразовать row в объект IDictionary<string, object>");
            }
        }

        public Table(string rowId, IEnumerable<object> rows)
            : this(rowId, false, rows)
        {
        }

        public Table(IEnumerable<object> rows)
            : this("rowNum", false, rows)
        {
        }

        public Table(bool ignoreZeroes, IEnumerable<object> rows)
            : this("rowNum", ignoreZeroes, rows)
        {
        }

        public Table()
            : this(Array.Empty<object>())
        {
        }
    }
}