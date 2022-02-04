using PasswordGeneratorOld.Models;
using System.Globalization;

namespace PasswordGeneratorOld.Services
{
    public class PasswordGenerateService
    {
        public ServiceResultForGP GeneratePasswordList(RequestModel request)
        {
            var response = new ServiceResultForGP();

            try
            {

                request = ControlOfMustConditions(request);
                request.Length = PasswordLengthOrganizer(request);
                if (request.Length < 0)
                    request = MinimalizeRequest(request);


                var passwordList = new List<string>();

                for (var j = 1; j <= request.Count; j++)
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

            if (request.Include != null)
            {
                foreach (var item in request.Include)
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
            if (request.Exclude != null)
            {
                foreach (var item in request.Exclude)
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

        //Düzenlenecek
        public RequestModel MinimalizeRequest(RequestModel request)
        {
            if (request.Length < 0)
            {
                if (request.LowerChars > 0 ||
                        request.Numbers > 0 ||
                        request.SpecialChars > 0 ||
                        request.UpperChars > 0)
                {
                    var total = request.Length +
                        request.LowerChars  +
                        request.UpperChars +
                        request.SpecialChars +
                        request.Numbers ;

                    if (total < 0)
                    {
                        request.Length = total;
                        request.LowerChars = 0;
                        request.Numbers = 0;
                        request.SpecialChars = 0;
                        request.UpperChars = 0;
                    }
                    else
                    {
                        var random = new Random();
                        var option = random.Next(0, 3);
                        switch (option)
                        {
                            case 0:
                                    if (request.UpperChars > 0)
                                    {
                                        request.UpperChars--;
                                        request.Length++;
                                    }
                                break;
                            case 1:                                
                                    if (request.LowerChars > 0)
                                    {
                                        request.LowerChars--;
                                        request.Length++;
                                    }
                                break;
                            case 2:                                
                                    if (request.SpecialChars > 0)
                                    {
                                        request.SpecialChars--;
                                        request.Length++;
                                    }
                                break;
                            case 3:                                
                                    if (request.Numbers > 0)
                                    {
                                        request.Numbers--;
                                        request.Length++;
                                    }
                                break;
                        }
                    } 
                    if (request.Length < 0)
                        request = MinimalizeRequest(request);
                }

                if (request.Length < 0)
                {
                    if(request.StartsWith !=null)
                    { 
                        var length = request.StartsWith.Length;                    
                        request.StartsWith = null;
                        request.Length = request.Length + length;
                    }
                }
                if (request.Length < 0)
                {
                    if(request.EndsWith != null)
                    {
                        var length = request.EndsWith.Length;
                        request.EndsWith = null;
                        request.Length = request.Length + length;
                    }
                }
                if (request.Length < 0)
                {
                    if(request.MustHave != null)
                    {
                        var length = request.MustHave.Length;
                        request.MustHave = null;
                        request.Length = request.Length + length;
                    }
                }
            }
            return request;
        }


        public int PasswordLengthOrganizer(RequestModel request)
        {
            var passwordLength = request.Length;

            if (request.StartsWith != null)
                passwordLength = passwordLength - request.StartsWith.Length;
            if (request.EndsWith != null)
                passwordLength = passwordLength - request.EndsWith.Length;
            if (request.MustHave != null)
                passwordLength = passwordLength - request.MustHave.Length;

            var charNumber = request.LowerChars + request.UpperChars
            + request.SpecialChars + request.Numbers;
            if(charNumber > passwordLength)
                passwordLength = passwordLength - charNumber;

            return passwordLength;
        }

        //Düzenlenecek
        public RequestModel OrganizeRequest(RequestModel request)
        {
            var length = 0;
            length = length + request.UpperChars + request.LowerChars + request.SpecialChars + request.Numbers;

            if (length < request.Length)
            {                
                var random = new Random();
                var option = random.Next(0, 3);
                switch (option)
                {
                    case 0:                        
                        request.UpperChars++;
                        break;
                    case 1:                        
                        request.LowerChars++;
                        break;
                    case 2:                        
                        request.Numbers++;
                        break;
                    case 3:
                        if (request.Length / 10 > request.SpecialChars)
                            request.SpecialChars++;
                        break;
                }
                request = OrganizeRequest(request);
            }
            else if(length> request.Length && request.Length != 0)
            {
                var random = new Random();
                var option = random.Next(0, 3);
                switch (option)
                {
                    case 0:                        
                        if (request.UpperChars > 0)
                            request.UpperChars--;
                        break;
                    case 1:                        
                        if (request.LowerChars > 0)
                            request.LowerChars--;
                        break;
                    case 2:
                         if (request.Numbers > 0)
                             request.Numbers--;
                        break;
                    case 3:
                        if (request.Length / 10 < request.SpecialChars && request.SpecialChars > 0)
                            request.SpecialChars--;
                        break;
                }
                request =OrganizeRequest(request);
            }
            return request;
        }


        public RequestModel ControlOfMustConditions(RequestModel request)
        {
            if (request.Exclude != null)
            {
                foreach (var item in request.Exclude)
                {
                    if (request.StartsWith != null)
                    {
                        if (request.StartsWith.IndexOf(item) != -1)                        
                            request.StartsWith = null;
                                                  
                    }
                    if (request.MustHave != null)
                    {
                        if (request.MustHave.IndexOf(item) != -1)
                            request.MustHave = null;
                    }
                    if (request.EndsWith != null)
                    {
                        if (request.EndsWith.IndexOf(item) != -1)
                            request.EndsWith = null;
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

            request = OrganizeRequest(request);

            if (request.UpperChars > 0)
            {
                for (var i = 1; i <= request.UpperChars; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        uppers[random.Next(uppers.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }
            if (request.LowerChars >0)
            {
                for (var i = 1; i <= request.LowerChars; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        lowers[random.Next(lowers.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }
            if (request.SpecialChars > 0)
            {
                for (var i = 1; i <= request.SpecialChars; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        specials[random.Next(specials.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }
            if (request.Numbers > 0)
            {
                for (var i = 1; i <= request.Numbers; i++)
                    response = response.Insert(
                        random.Next(response.Length),
                        number[random.Next(number.Length - 1)].ToString(CultureInfo.InvariantCulture)
                    );
            }


            if (request.MustHave != null)
                response = response.Insert(
                        random.Next(response.Length), request.MustHave);
            if (request.StartsWith != null)
                response = response.Insert(0, request.StartsWith);
            if (request.EndsWith != null)
                response = response.Insert(response.Length, request.EndsWith);


            return response;
        }

        public int? RequestCheck(string? word)
        {
            int? response;            
            try
            {
                response = Convert.ToInt32(word);
                if (response > 1000)
                    response = 1000;
            }
            catch (Exception)
            {
                response = null;
            }
            if(word == null) 
                response = null;
            return response;
        }
        public RequestModel PostRequestCheck(FirstRequestModel request)
        {
            var response = new RequestModel()
            {
                Length = (RequestCheck(request.Length) == null) ? 16 : (int)RequestCheck(request.Length),
                Count = (RequestCheck(request.Count) == null) ? 1 : (int)RequestCheck(request.Count),
                Numbers = (RequestCheck(request.Numbers) == null) ? 0 : (int)RequestCheck(request.Numbers),
                UpperChars = (RequestCheck(request.UpperChars) == null) ? 0 : (int)RequestCheck(request.UpperChars),
                LowerChars = (RequestCheck(request.LowerChars) == null) ? 0 : (int)RequestCheck(request.LowerChars),
                SpecialChars = (RequestCheck(request.SpecialChars) == null) ? 1 : (int)RequestCheck(request.SpecialChars),
                MustHave = request.MustHave,
                StartsWith = request.StartsWith,
                EndsWith = request.EndsWith,
                Include = request.Include,
                Exclude = request.Exclude,
                Type = request.Type
            };
            return response;
        }
    }
}
