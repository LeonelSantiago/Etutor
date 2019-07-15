using FluentValidation.Resources;
using FluentValidation.Validators;
using System;

namespace Etutor.Core.PropertyValidators
{
    public class ScalePrecisionValidator<T> : PropertyValidator where T : class, IEntityBase, new()
    {
        public ScalePrecisionValidator(int scale, int precision) : base(new LanguageStringSource(nameof(ScalePrecisionValidator)))
        {
            Init(scale, precision);
        }

        public int Scale { get; set; }

        public int Precision { get; set; }

        public bool? IgnoreTrailingZeros { get; set; }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var decimalValue = context.PropertyValue as decimal?;

            if (decimalValue.HasValue)
            {
                var scale = GetScale(decimalValue.Value);
                var precision = GetPrecision(decimalValue.Value);
                if (scale > Scale || precision > Precision)
                {
                    context.MessageFormatter
                        .AppendArgument("expectedPrecision", Precision)
                        .AppendArgument("expectedScale", Scale)
                        .AppendArgument("digits", precision - scale)
                        .AppendArgument("actualScale", scale);

                    return false;
                }
            }

            return true;
        }

        private void Init(int scale, int precision)
        {
            Scale = scale;
            Precision = precision;

            if (Scale < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(scale), $"Scale must be a positive integer. [value:{Scale}].");
            if (Precision < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(precision), $"Precision must be a positive integer. [value:{Precision}].");
            if (Precision < Scale)
                throw new ArgumentOutOfRangeException(
                    nameof(scale),
                    $"Scale must be less than precision. [scale:{Scale}, precision:{Precision}].");
        }

        private static UInt32[] GetBits(decimal Decimal)
        {
            // We want the integer parts as uint
            // C# doesn't permit int[] to uint[] conversion, but .NET does. This is somewhat evil...
            return (uint[])(object)decimal.GetBits(Decimal);
        }

        private static decimal GetMantissa(decimal Decimal)
        {
            var bits = GetBits(Decimal);
            return (bits[2] * 4294967296m * 4294967296m) + (bits[1] * 4294967296m) + bits[0];
        }

        private static uint GetUnsignedScale(decimal Decimal)
        {
            var bits = GetBits(Decimal);
            uint scale = (bits[3] >> 16) & 31;
            return scale;
        }

        private int GetScale(decimal Decimal)
        {
            uint scale = GetUnsignedScale(Decimal);
            if (IgnoreTrailingZeros.HasValue && IgnoreTrailingZeros.Value)
            {
                return (int)(scale - NumTrailingZeros(Decimal));
            }

            return (int)scale;
        }

        private static uint NumTrailingZeros(decimal Decimal)
        {
            uint trailingZeros = 0;
            uint scale = GetUnsignedScale(Decimal);
            for (decimal tmp = GetMantissa(Decimal); tmp % 10m == 0 && trailingZeros < scale; tmp /= 10)
            {
                trailingZeros++;
            }

            return trailingZeros;
        }

        private int GetPrecision(decimal Decimal)
        {
            // Precision: number of times we can divide by 10 before we get to 0        
            uint precision = 0;
            if (Decimal != 0m)
            {
                for (decimal tmp = GetMantissa(Decimal); tmp >= 1; tmp /= 10)
                {
                    precision++;
                }

                if (IgnoreTrailingZeros.HasValue && IgnoreTrailingZeros.Value)
                {
                    return (int)(precision - NumTrailingZeros(Decimal));
                }
            }
            else
            {
                // Handle zero differently. It's odd.
                precision = (uint)GetScale(Decimal) + 1;
            }

            return (int)precision;
        }
    }
}

