namespace Route256.PriceCalculator.Domain.Interfaces.Repository;

using Route256.PriceCalculator.Domain.Models;

public interface IGoodsRepository
{
    /// <summary>
    /// Добавляет или изменяет товар в хранилище.
    /// </summary>
    /// <param name="entity">Товар</param>
    void AddOrUpdate(GoodModel entity);

    /// <summary>
    /// Получает все наименования товаров из хранилища.
    /// </summary>
    ICollection<GoodModel> GetAll();

    /// <summary>
    /// Получает товар с определенным идентификатором из хранилища.
    /// </summary>
    /// <param name="id">Идентификатор товара в хранилище</param>
    GoodModel Get(int id);

    /// <summary>
    /// Определяет существование наименование товара в хранилище по id.
    /// </summary>
    /// <param name="id">Идентификатор товара в хранилище</param>
    bool ContainsById(int id);
}