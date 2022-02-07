

using PasswordGenerator.Models;
using System.Globalization;

namespace PasswordGenerator.Services
{
    public class PasswordGenerateService
    {
        public ServiceResultForGP GeneratePasswordList(RequestModel request)
        {
            var response = new ServiceResultForGP();

            try
            {

                request = ControlOfMustConditions(request);
                request.PasswordLength = PasswordLengthOrganizer(request);
                if (request.PasswordLength < 0)
                    request = MinimalizeRequest(request);


                var passwordList = new List<string>();

                for (var j = 1; j <= request.PasswordNumber; j++)
                {
                    var password = GeneratePassword(request);
                    passwordList.Add(password);
                }
                response.Data = passwordList;
                response.Success = true;

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = null;
                return response;
            }

        }

        public CharStrings OrganizeCharLists(RequestModel request)
        {
            var lowers = "abcdefghijkmnopqrstuvwxyz";
            var uppers = "ABCDEFGHJKLMNQPRSTUVWXYZ";
            var number = "0123456789";
            var specials = ".:-_@%&=;,*/+";

            if (request.CanContain != null)
            {
                foreach (var item in request.CanContain)
                {
                    if (Char.IsDigit(item))
                    {
                        if (number.IndexOf(item) == -1)
                            number = number + item;
                    }
                    else if (Char.IsLetter(item))
                    {
                        if (Char.IsUpper(item))
                        {
                            if (uppers.IndexOf(item) == -1)
                                uppers = uppers + item;
                        }
                        else
                        {
                            if (lowers.IndexOf(item) == -1)
                                lowers = lowers + item;
                        }
                    }
                    else
                    {
                        if (specials.IndexOf(item) == -1)
                            specials = specials + item;
                    }
                }

            }
            if (request.CanNotContain != null)
            {
                foreach (var item in request.CanNotContain)
                {
                    if (Char.IsDigit(item))
                    {
                        if (number.IndexOf(item) != -1)
                            number = number.Remove(number.IndexOf(item), 1);
                    }
                    else if (Char.IsLetter(item))
                    {
                        if (Char.IsUpper(item))
                        {
                            if (uppers.IndexOf(item) != -1)
                                uppers = uppers.Remove(uppers.IndexOf(item), 1);
                        }
                        else
                        {
                            if (lowers.IndexOf(item) != -1)
                                lowers = lowers.Remove(lowers.IndexOf(item), 1);
                        }
                    }
                    else
                    {
                        if (specials.IndexOf(item) != -1)
                            specials = specials.Remove(specials.IndexOf(item), 1);
                    }
                }
            }

            return new CharStrings()
            {
                Lowers = lowers,
                Uppers = uppers,
                Number = number,
                Specials = specials
            };

        }


        public RequestModel MinimalizeRequest(RequestModel request)
        {
            if (request.PasswordLength < 0)
            {
                if (request.MinimumNumberOfLowerCaseLetter > 0 ||
                        request.MinimumNumberOfNumericalCharacter > 0 ||
                        request.MinimumNumberOfSpecialCharacter > 0 ||
                        request.MinimumNumberOfUpperCaseLetter > 0)
                {
                    var total = request.PasswordLength +
                        (request.IsHaveLowerCaseLetter ? request.MinimumNumberOfLowerCaseLetter : 0) +
                        (request.IsHaveUpperCaseLetter ? request.MinimumNumberOfUpperCaseLetter : 0) +
                        (request.IsHaveSpecialCharacter ? request.MinimumNumberOfSpecialCharacter : 0) +
                        (request.IsHaveNumericalCharacter ? request.MinimumNumberOfNumericalCharacter : 0) ;

                    if (total < 0)
                    {
                        request.PasswordLength = total;
                        request.MinimumNumberOfLowerCaseLetter = 0;
                        request.MinimumNumberOfNumericalCharacter = 0;
                        request.MinimumNumberOfSpecialCharacter = 0;
                        request.MinimumNumberOfUpperCaseLetter = 0;
                    }
                    else
                    {
                        var random = new Random();
                        var option = random.Next(0, 4);
                        switch (option)
                        {
                            case 0:
                                if (request.IsHaveUpperCaseLetter)
                                    if (request.MinimumNumberOfUpperCaseLetter > 0)
                                    {
                                        request.MinimumNumberOfUpperCaseLetter--;
                                        request.PasswordLength++;
                                    }
                                break;
                            case 1:
                                if (request.IsHaveLowerCaseLetter)
                                    if (request.MinimumNumberOfLowerCaseLetter > 0)
                                    {
                                        request.MinimumNumberOfLowerCaseLetter--;
                                        request.PasswordLength++;
                                    }
                                break;
                            case 2:
                                if (request.IsHaveSpecialCharacter)
                                    if (request.MinimumNumberOfSpecialCharacter > 0)
                                    {
                                        request.MinimumNumberOfSpecialCharacter--;
                                        request.PasswordLength++;
                                    }
                                break;
                            case 3:
                                if (request.IsHaveNumericalCharacter)
                                    if (request.MinimumNumberOfNumericalCharacter > 0)
                                    {
                                        request.MinimumNumberOfNumericalCharacter--;
                                        request.PasswordLength++;
                                    }
                                break;
                        }

                    }
                    
                    

                    //if (request.IsHaveUpperCaseLetter)
                    //{
                    //    if (request.MinimumNumberOfUpperCaseLetter > 0)
                    //    {
                    //        request.MinimumNumberOfUpperCaseLetter--;
                    //        request.PasswordLength++;
                    //    }
                    //}
                    //if (request.IsHaveLowerCaseLetter)
                    //{
                    //    if (request.MinimumNumberOfLowerCaseLetter > 0)
                    //    {
                    //        request.MinimumNumberOfLowerCaseLetter--;
                    //        request.PasswordLength++;
                    //    }
                    //}
                    //if (request.IsHaveSpecialCharacter)
                    //{
                    //    if (request.MinimumNumberOfSpecialCharacter > 0)
                    //    {
                    //        request.MinimumNumberOfSpecialCharacter--;
                    //        request.PasswordLength++;
                    //    }
                    //}
                    //if (request.IsHaveNumericalCharacter)
                    //{
                    //    if (request.MinimumNumberOfNumericalCharacter > 0)
                    //    {
                    //        request.MinimumNumberOfNumericalCharacter--;
                    //        request.PasswordLength++;
                    //    }
                    //}


                    if (request.PasswordLength < 0)
                        request = MinimalizeRequest(request);

                }

                if (request.PasswordLength < 0)
                {
                    if(request.MustStartsWith !=null)
                    { 
                        var length = request.MustStartsWith.Length;                    
                        request.MustStartsWith = null;
                        request.PasswordLength = request.PasswordLength + length;
                    }
                }
                if (request.PasswordLength < 0)
                {
                    if(request.MustEndsWith != null)
                    {
                        var length = request.MustEndsWith.Length;
                        request.MustEndsWith = null;
                        request.PasswordLength = request.PasswordLength + length;
                    }
                }
                if (request.PasswordLength < 0)
                {
                    if(request.MustHave != null)
                    {
                        var length = request.MustHave.Length;
                        request.MustHave = null;
                        request.PasswordLength = request.PasswordLength + length;
                    }
                }
            }
            return request;
        }


        public int PasswordLengthOrganizer(RequestModel request)
        {
            var passwordLength = request.PasswordLength;

            if (request.MustStartsWith != null)
                passwordLength = passwordLength - request.MustStartsWith.Length;
            if (request.MustEndsWith != null)
                passwordLength = passwordLength - request.MustEndsWith.Length;
            if (request.MustHave != null)
                passwordLength = passwordLength - request.MustHave.Length;

            var charNumber = request.IsHaveLowerCaseLetter ? request.MinimumNumberOfLowerCaseLetter : 0;
            charNumber = charNumber + (request.IsHaveUpperCaseLetter ? request.MinimumNumberOfUpperCaseLetter : 0);
            charNumber = charNumber + (request.IsHaveSpecialCharacter ? request.MinimumNumberOfSpecialCharacter : 0);
            charNumber = charNumber + (request.IsHaveNumericalCharacter ? request.MinimumNumberOfNumericalCharacter : 0);
            if(charNumber > passwordLength)
                passwordLength = passwordLength - charNumber;

            return passwordLength;
        }


        public RequestModel OrganizeMinumumNumbers(RequestModel request)
        {
            var length = 0;
            if (request.IsHaveUpperCaseLetter)
                length = length + request.MinimumNumberOfUpperCaseLetter;
            if (request.IsHaveLowerCaseLetter)
                length = length + request.MinimumNumberOfLowerCaseLetter;
            if (request.IsHaveSpecialCharacter)
                length = length + request.MinimumNumberOfSpecialCharacter;
            if (request.IsHaveNumericalCharacter)
                length = length + request.MinimumNumberOfNumericalCharacter;
            if (length < request.PasswordLength)
            {
                if(request.IsHaveLowerCaseLetter == false &&
                    request.IsHaveUpperCaseLetter == false &&
                    request.IsHaveNumericalCharacter == false &&
                    request.IsHaveSpecialCharacter == false)
                {
                    request.IsHaveSpecialCharacter = true;
                    request.IsHaveUpperCaseLetter = true;
                    request.IsHaveNumericalCharacter = true;
                    request.IsHaveLowerCaseLetter = true;
                }
                var random = new Random();
                var option = random.Next(0, 4);
                switch (option)
                {
                    case 0:
                        if (request.IsHaveUpperCaseLetter)
                            request.MinimumNumberOfUpperCaseLetter++;
                        break;
                    case 1:
                        if (request.IsHaveLowerCaseLetter)
                            request.MinimumNumberOfLowerCaseLetter++;
                        break;
                    case 2:
                        if (request.IsHaveSpecialCharacter)
                            request.MinimumNumberOfSpecialCharacter++;
                        break;
                    case 3:
                        if (request.IsHaveNumericalCharacter)
                            request.MinimumNumberOfNumericalCharacter++;
                        break;

                    default:
                        break;
                }

                request = OrganizeMinumumNumbers(request);
            }
            else if(length> request.PasswordLength && request.PasswordLength != 0)
            {
                var random = new Random();
                var option = random.Next(0, 4);
                switch (option)
                {
                    case 0:
                        if (request.IsHaveUpperCaseLetter)
                            if (request.MinimumNumberOfUpperCaseLetter > 0)
                                request.MinimumNumberOfUpperCaseLetter--;
                        break;
                    case 1:
                        if (request.IsHaveLowerCaseLetter)
                            if (request.MinimumNumberOfLowerCaseLetter > 0)
                                request.MinimumNumberOfLowerCaseLetter--;
                        break;
                    case 2:
                        if (request.IsHaveSpecialCharacter)
                            if (request.MinimumNumberOfSpecialCharacter > 0)
                                request.MinimumNumberOfSpecialCharacter--;
                        break;
                    case 3:
                        if (request.IsHaveNumericalCharacter)
                            if (request.MinimumNumberOfNumericalCharacter > 0)
                                request.MinimumNumberOfNumericalCharacter--;
                        break;
                }
                request =OrganizeMinumumNumbers(request);
            }
            return request;
        }


        public RequestModel ControlOfMustConditions(RequestModel request)
        {
            if (request.CanNotContain != null)
            {
                foreach (var item in request.CanNotContain)
                {
                    if (request.MustStartsWith != null)
                    {
                        if (request.MustStartsWith.IndexOf(item) != -1)                        
                            request.MustStartsWith = null;
                                                  
                    }
                    if (request.MustHave != null)
                    {
                        if (request.MustHave.IndexOf(item) != -1)
                            request.MustHave = null;
                    }
                    if (request.MustEndsWith != null)
                    {
                        if (request.MustEndsWith.IndexOf(item) != -1)
                            request.MustEndsWith = null;
                    }
                }
            }
            return request;
        }

        public string GeneratePassword(RequestModel request)
        {
            var organizedCharLists = OrganizeCharLists(request);

            var lowers = organizedCharLists.Lowers;
            var uppers = organizedCharLists.Uppers;
            var number = organizedCharLists.Number;
            var specials = organizedCharLists.Specials;


            var response = "";
            var random = new Random();

            request = OrganizeMinumumNumbers(request);

            if (request.IsHaveUpperCaseLetter == true)
            {
                for (var i = 1; i <= request.MinimumNumberOfUpperCaseLetter; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        uppers[random.Next(uppers.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }
            if (request.IsHaveLowerCaseLetter == true)
            {
                for (var i = 1; i <= request.MinimumNumberOfLowerCaseLetter; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        lowers[random.Next(lowers.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }
            if (request.IsHaveSpecialCharacter == true)
            {
                for (var i = 1; i <= request.MinimumNumberOfSpecialCharacter; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        specials[random.Next(specials.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }
            if (request.IsHaveNumericalCharacter == true)
            {
                for (var i = 1; i <= request.MinimumNumberOfNumericalCharacter; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        number[random.Next(number.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }


            if (request.MustHave != null)
                response = response.Insert(
                        random.Next(response.Length), request.MustHave);
            if (request.MustStartsWith != null)
                response = response.Insert(0, request.MustStartsWith);
            if (request.MustEndsWith != null)
                response = response.Insert(response.Length, request.MustEndsWith);


            return response;
        }


    }
}
