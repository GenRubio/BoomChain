<?php

namespace App\Http\Controllers\Admin;

use App\Http\Requests\ObjetosCompradoRequest;
use Backpack\CRUD\app\Http\Controllers\CrudController;
use Backpack\CRUD\app\Library\CrudPanel\CrudPanelFacade as CRUD;
use Illuminate\Support\Facades\Route;

class ObjetosCompradoCrudController extends CrudController
{
    use \Backpack\CRUD\app\Http\Controllers\Operations\ListOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\CreateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\UpdateOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\DeleteOperation;
    use \Backpack\CRUD\app\Http\Controllers\Operations\ShowOperation;

    protected $user_id;

    public function setup()
    {
        CRUD::setModel(\App\Models\ObjetosComprado::class);
        $this->setRoute();
        $this->breadCrumbs();
        $this->filterList();
        CRUD::setEntityNameStrings('objetos', 'objetos');
    }

    private function setRoute()
    {
        $this->user_id = Route::current()->parameter('user_id');

        $this->crud->setRoute(
            "admin/usuario/"
                . $this->user_id . "/mochila-objetos"
        );
    }
    private function breadCrumbs()
    {
        $this->data['breadcrumbs'] = [
            trans('backpack::crud.admin') => backpack_url('dashboard'),

            'Usuarios' => backpack_url('usuario'),
            'Mochila' => backpack_url("usuario/"
                . $this->user_id
                . "/mochila-objetos"),
            trans('backpack::crud.list') => false,
        ];
    }

    private function filterList()
    {
        $this->crud->addClause('where', 'usuario_id', $this->user_id);
    }


    protected function setupListOperation()
    {
        $this->crud->addColumn([
            'name' => 'objeto',
            'label' => 'Nombre objeto',
            'type' => 'relationship',
            'attribute' => 'titulo',
            'model'     => App\Models\CatalagoObjeto::class,
        ]);
    }

    protected function setupCreateOperation()
    {
        CRUD::setValidation(ObjetosCompradoRequest::class);

        CRUD::setFromDb(); // fields

    }

    protected function setupUpdateOperation()
    {
        $this->setupCreateOperation();
    }
}
