using VideoRent.Data;
using VideoRent.Domain;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace VideoRent.Test;
[TestFixture]
public class PeliculaDataTest
{
    private String connectionString;
    [SetUp]
    public void Setup()
    {
        this.connectionString = "Data Source=AMENA\\SQLEXPRESS;Initial Catalog=videorent;User ID=rentinguser;Password=rentingUSER;Encrypt=False;Trust Server Certificate=True";
    }

    [Test]
    public async Task GetPeliculasPorTitulo_ReturnValues()
    {
        var peliculaData = new PeliculaData(this.connectionString);
        List<Pelicula> peliculas = await peliculaData.GetPeliculasPorTitulo("mat");
        Assert.That(peliculas.Count, Is.GreaterThan(0));
    }

    [Test]
    public void Insertar_ValidPelicula_InsertsSuccessfully()
    {
        // Arrange
        var peliculaData = new PeliculaData(this.connectionString);

        // Datos de prueba
        var genero = new Genero { GeneroId = 2090, NombreGenero = "Action" }; // Usar un GeneroId existente en b.d. de pruebas
        var actor1 = new Actor { ActorId = 1120, NombreActor = "Keanu", ApellidosActor ="Reeve" };    
        var peliculaToInsert = new Pelicula
        {
            Titulo = "Matrix 1", 
            Subtitulada = true,
            Estreno = false,
            Genero = genero,
            Actores = new List<Actor> { actor1 }
        };
        // Act
        Assert.DoesNotThrow(() => peliculaData.Insertar(peliculaToInsert),
            "Insertar una pelicula no debería generar una exepción");
        Assert.That(peliculaToInsert.PeliculaId, Is.GreaterThan(0));
    }
}
