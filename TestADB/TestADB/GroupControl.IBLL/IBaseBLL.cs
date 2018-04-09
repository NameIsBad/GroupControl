using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GroupControl.Model;

namespace GroupControl.IBLL
{
    public interface IBaseBLL<T1,T2> where T1 : class, new() where T2:BaseViewModel,new() 
    {

        ExcuteResultViewModel BatchInsert(IList<T1> model);

        ExcuteResultViewModel Insert(T1 model);

        ExcuteResultViewModel Update(T1 model);

        ExcuteResultViewModel Delete(T1 model);

        T1 GetModel(int id);

        T2 GetModel(T2 t);

        IList<T1> GetList(T2 viewModel);

        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        PagerViewModel<T1> GetPageList(T2 viewModel);

        /// <summary>
        /// 单表分页查询
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        PagerViewModel<T1> GetAsnyPageList(T2 viewModel);

        /// <summary>
        /// 多表查询分页
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        PagerViewModel<T2> GetPageListWithUnion(T2 viewModel);

        
    }
}
