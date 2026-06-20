using System.ComponentModel.DataAnnotations;


namespace Schuldenbuch.Core.DTOs.PersonDtos
{
	public class AddPersonDto
{
    public string Name { get; set; }


    public string Street { get; set; }


    public string ZipCode { get; set; }


    public string City { get; set; }
}

}
